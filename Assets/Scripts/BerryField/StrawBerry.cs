using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using Random = UnityEngine.Random;

public class StrawBerry : MonoBehaviour
{   
    private Animator anim;
    private SpriteRenderer sprite;
    private Vector2 pos;
    public Bug bug;

    public float createTime = 0f;
    public bool canGrow = true;
    public bool hasBug = false;

    public int berryIdx;    
    public int level;
    public int route = -1;
    public float randomTime = 0f;
    public float chance;
    public float[] berryProb = { 10f, 20f, 30f, 40f };   

    void Awake()
    {       
        anim = GetComponent<Animator>();
        sprite = GetComponent<SpriteRenderer>();
    }
    private void OnEnable()
    {
        randomTime = Random.Range(7.0f, 18.0f);
        pos = transform.position;
        DefineBerryRank();
        SetAnim(0);       
    }
    private void OnDisable()
    {
        // 변수 초기화
        canGrow = true;
        createTime = 0f;
        route = -1;
        randomTime = 0f;
        chance = 0f;        

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
            transform.position = new Vector2(pos.x, pos.y + 0.1f);
        }
        else if (this.level == 1)
        {
            sprite.sortingOrder = 0;
            transform.position = new Vector2(pos.x - 0.1f, pos.y + 0.3f);
        }
        else
        {
            transform.position = new Vector2(pos.x - 0.1f, pos.y + 0.4f);
        }
        anim.SetInteger("Level", level);
    }
    void SelectRoute()
    {        
        anim.SetInteger("Route", this.route);
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
        float cumulative = 0f, probSum = 0;
        
        for (int i = 0; i < berryProb.Length; i++)
        {
            probSum += berryProb[i];
        }
        chance = Random.Range(0, probSum);
        
        for (int i = 0; i < 4; i++)
        {
            cumulative += berryProb[i];
            if (chance <= cumulative)
            {
                route = 3 - i;
                break;
            }
        }
    }   
}
