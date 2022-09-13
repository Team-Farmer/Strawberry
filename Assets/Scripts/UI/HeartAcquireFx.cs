using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;


public class HeartAcquireFx : MonoBehaviour
{
    [SerializeField] Text amount;
    [SerializeField] RectTransform rect;


    public void Explosion(Vector2 from, Vector2 to, float explo_range)
    {

        Sequence sequence = DOTween.Sequence();
        sequence.Append(gameObject.GetComponent<RectTransform>().DOAnchorPos(from + Random.insideUnitCircle * explo_range, 0.25f).SetEase(Ease.OutCubic));
        sequence.Append(gameObject.GetComponent<RectTransform>().DOMove(to, 0.5f).SetEase(Ease.InCubic));
        sequence.OnComplete(() =>
        {
            gameObject.SetActive(false);
            Destroy(gameObject);
        });
    }

    public void Explosion2(Vector2 from, float explo_range)
    {
        Sequence sequence = DOTween.Sequence();
        sequence.Append(rect.DOJumpAnchorPos(from + Random.insideUnitCircle * explo_range, 130, 1, 0.5f).SetEase(Ease.OutCubic));
        sequence.Append(rect.GetComponent<Image>().DOFade(0, 0.5f));
        sequence.OnComplete(() =>
        {
            gameObject.SetActive(false);
            Destroy(gameObject);
        });
    }

    public void Coin(Vector2 from, float distance, int cost, string text, int num)
    {
        if (num == 0)
        {
            if (cost <= 9999)           // 0~9999까지 A
            {
                amount.text = text + cost.ToString() + " A";
            }
            else if (cost <= 9999999)   // 10000~9999999(=9999B)까지 B
            {
                cost /= 1000;
                amount.text = text + cost.ToString() + " B";
            }
            else                        // 그 외 C (최대 2100C)
            {
                cost /= 1000000;
                amount.text = text + cost.ToString() + " C";
            }
        }
        else
            amount.text = text + cost.ToString();


        rect.transform.position = from;
        Sequence seq = DOTween.Sequence()
        .Append(rect.GetComponent<Text>().DOFade(1, 0.3f))
        .Join(rect.DOAnchorPosX(distance, 0.5f))
        .Append(rect.GetComponent<Text>().DOFade(0, 0.3f))
        .OnComplete(() =>
        {
            ObjectPool.ReturnObject(this);
        });
    }


}