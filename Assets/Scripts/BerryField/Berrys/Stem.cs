using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using Random = UnityEngine.Random;

public class Stem : MonoBehaviour
{
    private Animator anim;
    private SpriteRenderer sprite;
    private Vector2 stemPos;

    private int seedAnimLevel;   
    private Bug stemBug;

    public int stemIdx;
    public Berry instantBerry;

    public const float STEM_LEVEL_0 = Globalvariable.STEM_LEVEL_0;
    public const float STEM_LEVEL_1 = Globalvariable.STEM_LEVEL_1;
    public const float STEM_LEVEL_2 = Globalvariable.STEM_LEVEL_2;
    public const float STEM_LEVEL_3 = Globalvariable.STEM_LEVEL_3;
    public const float STEM_LEVEL_MAX = Globalvariable.STEM_LEVEL_MAX;

    RainCtrl rainCtrl;
    void Awake()
    {
        anim = GetComponent<Animator>();
        sprite = GetComponent<SpriteRenderer>();
        stemBug = GameManager.instance.bugList[stemIdx];
        rainCtrl = GameObject.FindGameObjectWithTag("Rain").GetComponent<RainCtrl>();
    }
    private void OnEnable()
    {       
        DataController.instance.gameData.berryFieldData[stemIdx].isStemEnable = true;

        if (DataController.instance.gameData.berryFieldData[stemIdx].createTime >= STEM_LEVEL_1)
        {
            sprite.sortingOrder = 0;
        }
        else sprite.sortingOrder = 2;

        stemPos = transform.position;

        if (DataController.instance.gameData.berryFieldData[stemIdx].isPlant)
        {
            MakeBerry();
            updateStem();
            return;
        }  
        
        DataController.instance.gameData.berryFieldData[stemIdx].randomTime = Random.Range(STEM_LEVEL_1 + 2.0f, STEM_LEVEL_MAX - 2.0f);
        DefineBerry();
        MakeBerry();
        SetAnim(0);
    }
    private void OnDisable()
    {
        DataController.instance.gameData.berryFieldData[stemIdx].isStemEnable = false;
        // 변수 초기화       
        if (DataController.instance.gameData.berryFieldData[stemIdx].isPlant) return;

        DataController.instance.gameData.berryFieldData[stemIdx].canGrow = true;
        DataController.instance.gameData.berryFieldData[stemIdx].createTime = 0f;
        DataController.instance.gameData.berryFieldData[stemIdx].randomTime = 0f;

        // 딸기 트랜스폼 초기화
        transform.localPosition = Vector3.zero;
        transform.localRotation = Quaternion.identity;

        sprite.sortingOrder = 2;        
        instantBerry = null;
    }
    void Update() // 시간에 따라 딸기 성장
    {
        if (DataController.instance.gameData.berryFieldData[stemIdx].canGrow) 
        {
            DataController.instance.gameData.berryFieldData[stemIdx].createTime += Time.deltaTime * rainCtrl.mult;
            updateStem();
        }
    }
    private void updateStem()
    {      
        if (DataController.instance.gameData.berryFieldData[stemIdx].randomTime <= DataController.instance.gameData.berryFieldData[stemIdx].createTime)
        {
            stemBug.GenerateBug();
            DataController.instance.gameData.berryFieldData[stemIdx].randomTime = 200f;
        }
        if (STEM_LEVEL_1 <= DataController.instance.gameData.berryFieldData[stemIdx].createTime && DataController.instance.gameData.berryFieldData[stemIdx].createTime < STEM_LEVEL_2)
        {
            if (seedAnimLevel == 1) return;
            SetAnim(1);
        }
        else if (STEM_LEVEL_2 <= DataController.instance.gameData.berryFieldData[stemIdx].createTime && DataController.instance.gameData.berryFieldData[stemIdx].createTime < STEM_LEVEL_3)
        {
            if (seedAnimLevel == 2) return;
            SetAnim(2);
        }
        else if (STEM_LEVEL_3 <= DataController.instance.gameData.berryFieldData[stemIdx].createTime && DataController.instance.gameData.berryFieldData[stemIdx].createTime < STEM_LEVEL_MAX)
        {
            if (seedAnimLevel == 3) return;
            SetAnim(3);
        }
        else if (DataController.instance.gameData.berryFieldData[stemIdx].createTime >= STEM_LEVEL_MAX)
        {            
            DataController.instance.gameData.berryFieldData[stemIdx].canGrow = false;
            if(!GameManager.instance.isblackPanelOn)
                GameManager.instance.farmList[stemIdx].GetComponent<BoxCollider2D>().enabled = true; // 밭의 콜라이더 다시 활성화

            SetAnim(4);
        }
    }
    public void SetAnim(int level)
    {
        this.seedAnimLevel = level;
        anim.SetInteger("Seed", seedAnimLevel);

        if (this.seedAnimLevel == 0)
        {
            transform.position = new Vector2(stemPos.x, stemPos.y + 0.02f);
        }
        else if (this.seedAnimLevel == 1)
        {
            sprite.sortingOrder = 0;
            transform.position = new Vector2(stemPos.x - 0.12f, stemPos.y + 0.24f);
        }
        else if (this.seedAnimLevel == 2)
        {           
            transform.position = new Vector2(stemPos.x - 0.15f, stemPos.y + 0.27f);
            instantBerry.gameObject.SetActive(true);
            instantBerry.GetComponent<Animator>().SetInteger("berryLevel", level);            
        }
        else if (this.seedAnimLevel == 3)
        {            
            transform.position = new Vector2(stemPos.x - 0.15f, stemPos.y + 0.29f);
            instantBerry.gameObject.SetActive(true);
            instantBerry.GetComponent<Animator>().SetInteger("berryLevel", level);
            instantBerry.transform.position = new Vector3(transform.position.x + 0.29f, transform.position.y, transform.position.z);
        }
        else if (this.seedAnimLevel == 4)
        {          
            transform.position = new Vector2(stemPos.x - 0.15f, stemPos.y + 0.35f);
            instantBerry.gameObject.SetActive(true);
            instantBerry.GetComponent<SpriteRenderer>().sprite = Globalvariable.instance.berryListAll[DataController.instance.gameData.berryFieldData[stemIdx].berryPrefabNowIdx].GetComponent<SpriteRenderer>().sprite;
            instantBerry.GetComponent<Animator>().SetInteger("berryLevel", level);
            instantBerry.transform.position = new Vector2(transform.position.x + 0.32f, transform.position.y + 0.06f);
        }
    }
    void DefineBerry() // 누적 확률변수로 랜덤한 딸기 생성
    {
        int cumulative = 0, probSum = 0;
        int len = Globalvariable.instance.berryListAll.Count;

        for (int i = 0; i < len; i++)
        {
            if(Globalvariable.instance.berryListAll[i] != null)
            {
                probSum += Globalvariable.instance.berryListAll[i].GetComponent<Berry>().berrykindProb; // 해금된 딸기에서 딸기의 발생확률을 가져옴
            }           
        }
        int berryRandomChance = Random.Range(0, probSum + 1);

        for (int i = 0; i < len; i++)
        {
            if (Globalvariable.instance.berryListAll[i] != null)
            {
                cumulative += Globalvariable.instance.berryListAll[i].GetComponent<Berry>().berrykindProb; // 해금된 딸기에서 딸기의 발생확률을 가져옴
                if (berryRandomChance <= cumulative)
                {
                    DataController.instance.gameData.berryFieldData[stemIdx].berryPrefabNowIdx = i;
                    break;
                }
            }                       
        }
    }
    void MakeBerry() // 딸기 생성
    {
        // 로드될때는 없어졌으니깐 로드 클래스에서 다시 생성해야됨(MakeBerry() 호출하면 됨)
        GameObject instantBerryObj = Instantiate(Globalvariable.instance.berryListAll[DataController.instance.gameData.berryFieldData[stemIdx].berryPrefabNowIdx], this.transform);
        instantBerryObj.name = Globalvariable.instance.berryListAll[DataController.instance.gameData.berryFieldData[stemIdx].berryPrefabNowIdx].name;

        instantBerry = instantBerryObj.GetComponent<Berry>();
        instantBerry.transform.position = new Vector3(transform.position.x + 0.22f, transform.position.y - 0.01f, transform.position.z);
        instantBerry.gameObject.SetActive(false);
    }
}
