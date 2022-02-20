using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using Random = UnityEngine.Random;

public class StrawBerry : MonoBehaviour
{
    public Transform berryTrans;
    private Animator anim;
    private SpriteRenderer sprite;
    private Vector2 stemPos;
    public Bug bug;

    Dictionary<string, int> Classic = new Dictionary<string, int>()
    {
        {"SeolHyang", 100 }
    };
    Dictionary<string, int> Special = new Dictionary<string, int>()
    {
        {"MaeHyang", 50 },
        {"PermanentSnow", 35 },
        {"KingsBerry", 15 },
    };
    Dictionary<string, int> Unique = new Dictionary<string, int>()
    {
        {"Choco", 100 }
    };

    List<Dictionary<string, int>> berryKindProb = new List<Dictionary<string, int>>();

    public float createTime = 0f;
    public bool canGrow = true;
    public bool hasBug = false;

    public int berryIdx;    
    public int level;
    public int kind = -1;
    public int rank = -1;
    public float randomTime = 0f;
    public int rankChance;
    public int kindChance;

    public int[] berryRankProb = { 50, 35, 15 };

    void Awake()
    {       
        anim = GetComponent<Animator>();
        sprite = GetComponent<SpriteRenderer>();

        berryKindProb.Add(Classic);
        berryKindProb.Add(Special);
        berryKindProb.Add(Unique);
    }
    private void OnEnable()
    {
        randomTime = Random.Range(7.0f, 18.0f);
        stemPos = transform.position;       
        DefineBerryKind();
        SetAnim(0);       
    }
    private void OnDisable()
    {
        // 변수 초기화
        canGrow = true;
        createTime = 0f;
        kind = -1;
        rank = -1;
        randomTime = 0f;
        rankChance = 0;
        kindChance = 0;

        // 딸기 트랜스폼 초기화
        transform.localPosition = Vector3.zero;
        transform.localRotation = Quaternion.identity;
        transform.localScale = Vector3.zero;       
    }   
    void Update() // 시간에 따라 딸기 성장
    {
        if (canGrow)
        {
            createTime += Time.deltaTime;
            if(randomTime <= createTime)
            {
                bug.GenerateBug();
                randomTime = 200f;
            }

            if (5.0f <= createTime && createTime < 10.0f)
            {
                if (level == 1) return;
                SetAnim(1);
            }               
            else if (10.0f <= createTime && createTime < 15.0f)
            {
                if (level == 2) return;
                SetAnim(2);
            }
            else if (15.0f <= createTime && createTime < 20.0f)
            {
                if (level == 3) return;
                SetAnim(3);
                         
            }
            else if (createTime >= 20.0f)
            {
                SelectRoute();
                canGrow = false;
                GameManager.instance.farmList[berryIdx].GetComponent<BoxCollider2D>().enabled = true; // 밭의 콜라이더 다시 활성화
            }
        }
    }
    public void SetAnim(int level)
    {        
        this.level = level;
        
        if (this.level == 0)
        {
            transform.position = new Vector2(stemPos.x, stemPos.y + 0.05f);
        }
        else if (this.level == 1)
        {
            sprite.sortingOrder = 0;
            transform.position = new Vector2(stemPos.x - 0.1f, stemPos.y + 0.3f);
        }
        else if(this.level == 2)
        {
            transform.position = new Vector2(stemPos.x - 0.1f, stemPos.y + 0.38f);            
        }
        else if (this.level == 3)
        {           
            berryTrans.position = new Vector2(transform.position.x + 0.48f, transform.position.y - 0.05f);
        }
        anim.SetInteger("Level", level);
    }
    void SelectRoute()
    {        
        anim.SetInteger("Kind", this.kind);
        anim.SetInteger("Rank", this.rank);
    }
    public void Explosion(Vector2 from, Vector2 to, float exploRange) // DOTWeen 효과
    {
        transform.position = from;
        Sequence sequence = DOTween.Sequence();
        sequence.Append(transform.DOMove(from + Random.insideUnitCircle * exploRange, 0.25f).SetEase(Ease.OutCubic));
        sequence.Append(transform.DOMove(to, 0.5f).SetEase(Ease.InCubic));
        sequence.AppendCallback(() => { gameObject.SetActive(false); });
    }
    void DefineBerryRank() // 누적 확률변수로 랜덤한 딸기 생성
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
    }
}
