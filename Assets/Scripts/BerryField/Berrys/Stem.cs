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
    //private Vector2 stemPos;
    [SerializeField]
    private Vector3 farmPos;
    private int seedAnimLevel;   
    private Bug stemBug;

    public int stemIdx;
    private GameObject instantBerryObj;

    /*public const float STEM_LEVEL_0 = Globalvariable.STEM_LEVEL_0;
    public const float STEM_LEVEL_1 = Globalvariable.STEM_LEVEL_1;
    public const float STEM_LEVEL_2 = Globalvariable.STEM_LEVEL_2;
    public const float STEM_LEVEL_3 = Globalvariable.STEM_LEVEL_3;
    public const float STEM_LEVEL_MAX = Globalvariable.STEM_LEVEL_MAX;*/

    RainCtrl rainCtrl;
    Globalvariable global;
    void Awake()
    {
        seedAnimLevel = 0;
        anim = GetComponent<Animator>();
        sprite = GetComponent<SpriteRenderer>();
        stemBug = GameManager.instance.bugList[stemIdx];

        rainCtrl = GameObject.FindGameObjectWithTag("Rain").GetComponent<RainCtrl>();
        global = GameObject.FindGameObjectWithTag("Global").GetComponent<Globalvariable>();
        //farmPos = new Vector3(GetComponentInParent<Farm>().transform.localPosition.x, GetComponentInParent<Farm>().transform.localPosition.y, 0);
    }
    private void OnEnable()
    {       

        DataController.instance.gameData.berryFieldData[stemIdx].isStemEnable = true;
        
        if (DataController.instance.gameData.berryFieldData[stemIdx].createTime >= DataController.instance.gameData.stemLevel[1])
        {
            sprite.sortingOrder = 0;
        }
        else sprite.sortingOrder = 2;

        

        if (DataController.instance.gameData.berryFieldData[stemIdx].isPlant)
        {
            MakeBerry();
            updateStem();
            return;
        }  
        
        DataController.instance.gameData.berryFieldData[stemIdx].randomTime = 
            Random.Range(DataController.instance.gameData.stemLevel[1] + 2.0f, DataController.instance.gameData.stemLevel[4] - 2.0f);
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
        transform.localPosition = new Vector3(0, 0.4f, 0);
        transform.localRotation = Quaternion.identity;

        sprite.sortingOrder = 2;
        Destroy(instantBerryObj);
        //instantBerryObj = null;
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
        if (DataController.instance.gameData.stemLevel[1] <= DataController.instance.gameData.berryFieldData[stemIdx].createTime
            && DataController.instance.gameData.berryFieldData[stemIdx].createTime < DataController.instance.gameData.stemLevel[2])
        {
            if (seedAnimLevel == 1) return; // seedAnimLevel도 저장해서 하면 될듯???
            transform.localPosition = new Vector3(0, 0.27f, 0);
            SetAnim(1);
        }
        else if (DataController.instance.gameData.stemLevel[2] <= DataController.instance.gameData.berryFieldData[stemIdx].createTime 
            && DataController.instance.gameData.berryFieldData[stemIdx].createTime < DataController.instance.gameData.stemLevel[3])
        {
            if (seedAnimLevel == 2) return;
            transform.localPosition = new Vector3(0, 0.40f, 0);
            SetAnim(2);
        }
        else if (DataController.instance.gameData.stemLevel[3] <= DataController.instance.gameData.berryFieldData[stemIdx].createTime
            && DataController.instance.gameData.berryFieldData[stemIdx].createTime < DataController.instance.gameData.stemLevel[4])
        {
            if (seedAnimLevel == 3) return;
            transform.localPosition = new Vector3(0, 0.40f, 0);
            SetAnim(3);
        }
        else if (DataController.instance.gameData.berryFieldData[stemIdx].createTime >= DataController.instance.gameData.stemLevel[4])
        {            
            DataController.instance.gameData.berryFieldData[stemIdx].canGrow = false;
            if(!GameManager.instance.isblackPanelOn)
                GameManager.instance.farmList[stemIdx].GetComponent<BoxCollider2D>().enabled = true; // 밭의 콜라이더 다시 활성화
            transform.localPosition = new Vector3(0, 0.35f, 0);
            SetAnim(4);
        }
    }
    public void SetAnim(int level)
    {
        this.seedAnimLevel = level;
        anim.SetInteger("Seed", seedAnimLevel);

        if (this.seedAnimLevel == 0)
        {
            //transform.position = new Vector2(farmPos.x, farmPos.y + 0.02f);
        }
        else if (this.seedAnimLevel == 1)
        {
            sprite.sortingOrder = 0;       
        }
        else if (this.seedAnimLevel == 2)
        {                      
            //instantBerry.gameObject.SetActive(true);
            //instantBerry.GetComponent<Animator>().SetInteger("berryLevel", level);
            //instantBerry.transform.localPosition = new Vector3(transform.localPosition.x + 0.3f, transform.localPosition.y - 0.5f, transform.localPosition.z);
        }
        else if (this.seedAnimLevel == 3)
        {                      
            //instantBerry.gameObject.SetActive(true);
            //instantBerry.GetComponent<Animator>().SetInteger("berryLevel", level);
            //instantBerry.transform.localPosition = new Vector3(transform.localPosition.x + 0.3f, transform.localPosition.y - 0.48f, transform.localPosition.z);
        }
        else if (this.seedAnimLevel == 4)
        {
            instantBerryObj.gameObject.SetActive(true);
            instantBerryObj.GetComponent<SpriteRenderer>().sprite = global.berryListAll[DataController.instance.gameData.berryFieldData[stemIdx].berryPrefabNowIdx].GetComponent<SpriteRenderer>().sprite;
            instantBerryObj.GetComponent<Animator>().SetInteger("berryLevel", level);
            instantBerryObj.transform.localPosition = new Vector2(transform.localPosition.x + 0.32f, transform.localPosition.y - 0.32f);
        }
    }
    void DefineBerry() // 누적 확률변수로 랜덤한 딸기 생성
    {
        int cumulative = 0, probSum = 0;
        int len = global.berryListAll.Count;

        for (int i = 0; i < len; i++)
        {
            if(DataController.instance.gameData.isBerryUnlock[i])
            {
                probSum += global.berryListAll[i].GetComponent<Berry>().berrykindProb; // 해금된 딸기에서 딸기의 발생확률을 가져옴
            }           
        }
        int berryRandomChance = Random.Range(0, probSum + 1);

        for (int i = 0; i < len; i++)
        {
            if (DataController.instance.gameData.isBerryUnlock[i])
            {
                cumulative += global.berryListAll[i].GetComponent<Berry>().berrykindProb; // 해금된 딸기에서 딸기의 발생확률을 가져옴
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

        instantBerryObj = Instantiate(global.berryListAll[DataController.instance.gameData.berryFieldData[stemIdx].berryPrefabNowIdx], this.transform);

        instantBerryObj.name = global.berryListAll[DataController.instance.gameData.berryFieldData[stemIdx].berryPrefabNowIdx].name;     
        instantBerryObj.gameObject.SetActive(false);
    }

    public GameObject getInstantBerryObj()
    {
        return this.instantBerryObj;
    }
}
