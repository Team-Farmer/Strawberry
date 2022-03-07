using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using Random = UnityEngine.Random;

public class Stem : MonoBehaviour
{
    //private Transform berryTrans;
    private Animator anim;
    private SpriteRenderer sprite;
    public Bug bug;
    
    // 프리팹 리스트가 필요함 이거는 직접 꺼내쓸 것이므로 해금이 된 딸기만 들어와야함.
    // 따라서 전체 프리팹리스트, 해금된 프리팹 리스트 둘 다 필요함
    


    private Vector2 stemPos;

    //
    public float createTime = 0f;
    public bool canGrow = true;
    public bool hasBug = false;
    public GameObject berryPrefabNow;
    public Berry instantBerry;

    public int berryIdx;
    public int seedAnimLevel; // 현재 레벨 저장해서 게임 데이터에 넘겨줘야함
    public int kind = -1;
    
    public float randomTime = 0f;
    public int berryRandomChance;
    public int kindChance;

    public int[] berryRankProb = { 50, 35, 15 }; //위에 다 옮김

    void Awake()
    {
        anim = GetComponent<Animator>();
        sprite = GetComponent<SpriteRenderer>();

        //berryTrans.transform.position = new Vector2(transform.position.x + 0.37f, transform.position.y - 0.02f);
        
    }
    private void OnEnable()
    {
        stemPos = transform.position;
        randomTime = Random.Range(7.0f, 18.0f);
        DefineBerry();
        MakeBerry();
        SetAnim(0);
    }
    private void OnDisable()
    {
        // 변수 초기화
        canGrow = true;
        createTime = 0f;
        kind = -1;
        
        randomTime = 0f;
        berryRandomChance = 0;
        kindChance = 0;

        // 딸기 트랜스폼 초기화
        transform.localPosition = Vector3.zero;
        //berryTrans.transform.localPosition = new Vector2(0.42f, 0.02f);
        transform.localRotation = Quaternion.identity;
        transform.localScale = Vector3.one;

        sprite.sortingOrder = 2;
        berryPrefabNow = null;
    }
    void Update() // 시간에 따라 딸기 성장
    {
        if (canGrow)
        {
            createTime += Time.deltaTime;
            if (randomTime <= createTime)
            {
                //bug.GenerateBug();임시비활성화
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
                GameManager.instance.farmList[berryIdx].GetComponent<BoxCollider2D>().enabled = true; // 밭의 콜라이더 다시 활성화
            }
        }
    }
    public void SetAnim(int level)
    {
        this.seedAnimLevel = level;
        anim.SetInteger("Seed", seedAnimLevel);

        if (this.seedAnimLevel == 0)
        {
            transform.position = new Vector2(stemPos.x, stemPos.y + 0.05f);
        }
        else if (this.seedAnimLevel == 1)
        {
            sprite.sortingOrder = 0;
            transform.position = new Vector2(stemPos.x - 0.12f, stemPos.y + 0.26f);
        }
        else if (this.seedAnimLevel == 2)
        {
            transform.position = new Vector2(stemPos.x - 0.15f, stemPos.y + 0.32f);
            instantBerry.gameObject.SetActive(true);
            instantBerry.GetComponent<Animator>().SetInteger("berryLevel", level);
            instantBerry.transform.position = new Vector2(transform.position.x + 0.3f, transform.position.y + 0.1f);            
        }
        else if (this.seedAnimLevel == 3)
        {
            transform.position = new Vector2(stemPos.x - 0.15f, stemPos.y + 0.35f);
            instantBerry.gameObject.SetActive(true);
            instantBerry.GetComponent<Animator>().SetInteger("berryLevel", level);
        }
        else if (this.seedAnimLevel == 4)
        {
            transform.position = new Vector2(stemPos.x - 0.15f, stemPos.y + 0.35f);
            instantBerry.gameObject.SetActive(true);
            instantBerry.GetComponent<Animator>().SetInteger("berryLevel", level);
        }
    }
    /*void SelectRoute()
    {
        anim.SetInteger("Kind", this.kind);
        anim.SetInteger("Rank", this.rank);
    }*/
    void DefineBerry() // 누적 확률변수로 랜덤한 딸기 생성
    {
        int cumulative = 0, probSum = 0;
        int len = GameManager.instance.berryPrefabListUnlock.Count;
        
        for (int i = 0; i < len; i++)
        {
            probSum += GameManager.instance.berryPrefabListUnlock[i].GetComponent<Berry>().berrykindProb; // 해금된 딸기에서 딸기의 발생확률을 가져옴
        }
        berryRandomChance = Random.Range(0, probSum + 1);

        for (int i = 0; i < len; i++)
        {
            cumulative += GameManager.instance.berryPrefabListUnlock[i].GetComponent<Berry>().berrykindProb;
            if (berryRandomChance <= cumulative)
            {
                berryPrefabNow = GameManager.instance.berryPrefabListUnlock[i];               
                break;
            }
        }
    }
    void MakeBerry() // 딸기 생성
    {
        GameObject instantBerryObj = Instantiate(berryPrefabNow, this.transform);
        instantBerryObj.name = berryPrefabNow.name;

        instantBerry = instantBerryObj.GetComponent<Berry>();
        instantBerry.gameObject.SetActive(false);
    }
    /*void DefineBerryRank1() // 누적 확률변수로 랜덤한 딸기 생성
    {
        int cumulative = 0, probRankSum = 0;
        int rankLen = berryRankProb.Length;

        for (int i = 0; i < rankLen; i++)
        {
            probRankSum += berryRankProb[i];
        }
        rankChance = Random.Range(0, probRankSum + 1);

        for (int i = 0; i < rankLen; i++)
        {
            cumulative += berryRankProb[i];
            if (rankChance <= cumulative)
            {
                rank = i;
                break;
            }
        }
    }
    void DefineBerryKind()
    {
        DefineBerryRank();
        int cumulative = 0, probKindSum = 0;
        int kindLen = berryKindProb[rank].Count;

        foreach (int value in berryKindProb[rank].Values)
        {
            probKindSum += value;
        }
        kindChance = Random.Range(0, probKindSum + 1);
        int i = 0;
        foreach (int value in berryKindProb[rank].Values)
        {
            cumulative += value;
            if (kindChance <= cumulative)
            {
                kind = i;
                break;
            }
            i++;
        }
    }*/
}
