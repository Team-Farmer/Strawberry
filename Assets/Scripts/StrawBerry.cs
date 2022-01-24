using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using Random = UnityEngine.Random;

public class StrawBerry : MonoBehaviour
{
    public float createTime = 0f;
    public bool canGrow = true;
    Animator anim;
   
    void Awake()
    {
        anim = GetComponent<Animator>();
    }
    private void OnEnable()
    {
        SetAnim(0);
    }
    private void OnDisable()
    {
        // 변수 초기화
        canGrow = true;
        createTime = 0f;

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
                SetAnim(1);
            else if (10.0f <= createTime && createTime < 15.0f)
                SetAnim(2);
            else if (15.0f <= createTime && createTime < 20.0f)
                SetAnim(3);
            else if (createTime >= 20.0f)
            {
                SetAnim(4);
                canGrow = false;
            }
        }
    }
    public void SetAnim(int level)
    {
        anim.SetInteger("Level", level);
    }
    public void Explosion(Vector2 from, Vector2 to, float explo_range) // DOTWeen 효과
    {
        transform.position = from;
        Sequence sequence = DOTween.Sequence();
        sequence.Append(transform.DOMove(from + Random.insideUnitCircle * explo_range, 0.25f).SetEase(Ease.OutCubic));
        sequence.Append(transform.DOMove(to, 0.5f).SetEase(Ease.InCubic));
        sequence.AppendCallback(() => { gameObject.SetActive(false); });
    }
}
