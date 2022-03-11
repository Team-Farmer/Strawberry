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

    //
    public float createTime = 0f;
    public bool canGrow = true;
    public bool hasBug = false;
    
    public int stemIdx;
    public int berryPrefabNowIdx;
    public bool isStemEnable; // 얘 일때만 만들어주면 되니까
    public float randomTime = 0f;

    public int[] berryRankProb = { 50, 35, 15 }; // 얘는 데이터 클래스에 들어갈 내용은 아님
    public int seedAnimLevel; // 안넘겨줘야됨 어차피 creatTime 넘기면 알아서 조정되는 변수임 아마?
    public GameObject berryPrefabNow;
    public Berry instantBerry;
    public Bug stemBug;
    public Farm stemFarm;

    void Awake()
    {
        anim = GetComponent<Animator>();
        sprite = GetComponent<SpriteRenderer>();
        stemBug = GameManager.instance.bugList[stemIdx];
        stemFarm = GameManager.instance.farmList[stemIdx];
    }
    private void OnEnable()
    {
        isStemEnable = true;

        if (createTime >= 5.0f) sprite.sortingOrder = 0;
        else sprite.sortingOrder = 2;

        if (stemFarm.isPlant) return;
       
        stemPos = transform.position;
        randomTime = Random.Range(7.0f, 18.0f);
        DefineBerry();
        MakeBerry();
        SetAnim(0);
    }
    private void OnDisable()
    {
        isStemEnable = false;
        // 변수 초기화       
        if (stemFarm.isPlant) return;

        canGrow = true;
        createTime = 0f;
        randomTime = 0f;

        // 딸기 트랜스폼 초기화
        transform.localPosition = Vector3.zero;
        transform.localRotation = Quaternion.identity;

        sprite.sortingOrder = 2;
        berryPrefabNow = null;
        instantBerry = null;
    }
    void Update() // 시간에 따라 딸기 성장
    {
        if (canGrow)
        {
            createTime += Time.deltaTime;
            if (randomTime <= createTime)
            {
                stemBug.GenerateBug();
                randomTime = 200f;
            }
            if (5.0f <= createTime && createTime < 10.0f)
            {
                if (seedAnimLevel == 1) return;
                SetAnim(1);
            }
            else if (10.0f <= createTime && createTime < 15.0f)
            {
                if (seedAnimLevel == 2) return;

                SetAnim(2);
            }
            else if (15.0f <= createTime && createTime < 20.0f)
            {
                if (seedAnimLevel == 3) return;
                SetAnim(3);
            }
            else if (createTime >= 20.0f)
            {
                SetAnim(4);
                canGrow = false;
                GameManager.instance.farmList[stemIdx].GetComponent<BoxCollider2D>().enabled = true; // 밭의 콜라이더 다시 활성화
            }
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
            //instantBerry.transform.position = new Vector2(transform.position.x + 0.3f, transform.position.y + 0.1f);
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
            instantBerry.GetComponent<Animator>().SetInteger("berryLevel", level);
            instantBerry.transform.position = new Vector2(transform.position.x + 0.32f, transform.position.y + 0.06f);
        }
    }

    void DefineBerry() // 누적 확률변수로 랜덤한 딸기 생성
    {
        int cumulative = 0, probSum = 0;
        int len = GameManager.instance.berryPrefabListUnlock.Count;

        for (int i = 0; i < len; i++)
        {
            probSum += GameManager.instance.berryPrefabListUnlock[i].GetComponent<Berry>().berrykindProb; // 해금된 딸기에서 딸기의 발생확률을 가져옴
        }
        int berryRandomChance = Random.Range(0, probSum + 1);

        for (int i = 0; i < len; i++)
        {
            cumulative += GameManager.instance.berryPrefabListUnlock[i].GetComponent<Berry>().berrykindProb;
            if (berryRandomChance <= cumulative)
            {
                berryPrefabNowIdx = i;
                berryPrefabNow = GameManager.instance.berryPrefabListUnlock[berryPrefabNowIdx];
                break;
            }
        }
    }
    void MakeBerry() // 딸기 생성
    {
        // 로드될때는 없어졌으니깐 로드 클래스에서 다시 생성해야됨(MakeBerry() 호출하면 됨)
        GameObject instantBerryObj = Instantiate(GameManager.instance.berryPrefabListUnlock[berryPrefabNowIdx], this.transform);
        instantBerryObj.name = berryPrefabNow.name;

        instantBerry = instantBerryObj.GetComponent<Berry>();
        instantBerry.transform.position = new Vector3(transform.position.x + 0.22f, transform.position.y - 0.01f, transform.position.z);
        instantBerry.gameObject.SetActive(false);
    }
}
