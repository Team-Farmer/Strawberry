using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using Random = UnityEngine.Random;

public class StrawBerry : MonoBehaviour
{   
    private Animator anim;
    private Vector2 pos;
    
    public float createTime = 0f;
    public bool canGrow = true;

    public int berryIdx;    
    public int level;
    public int route = -1;

    public float[] berryProb = { 10f, 20f, 30f, 40f };

    void Awake()
    {
        anim = GetComponent<Animator>();       
    }
    private void OnEnable()
    {
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
            }
        }
    }
    public void SetAnim(int level)
    {
        this.level = level;
        if(this.level == 0)
        {
            transform.position = new Vector2(pos.x, pos.y + 0.1f);
        }
        else if (this.level == 1)
        {
            transform.position = new Vector2(pos.x - 0.1f, pos.y + 0.35f);
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
        float probSum = 0f, cumulative = 0f;
        float chance;
        for (int i = 0; i < berryProb.Length; i++)
        {
            probSum += berryProb[i];
        }
        chance = Random.Range(0, probSum + 1);
        
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
