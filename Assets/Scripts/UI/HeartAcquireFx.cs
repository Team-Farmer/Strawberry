using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;


public class HeartAcquireFx : MonoBehaviour
{
    public RectTransform rect;
    public void Explosion(Vector2 from, Vector2 to, float explo_range)
    {
        rect.position = from;
        Sequence sequence = DOTween.Sequence();
        sequence.Append(rect.DOAnchorPos(from + Random.insideUnitCircle * explo_range, 0.25f).SetEase(Ease.OutCubic));
        sequence.Append(rect.DOMove(to, 0.5f).SetEase(Ease.InCubic));
        sequence.AppendCallback(() =>
        {
            gameObject.SetActive(false);
            Destroy(gameObject);
        });
    }

    public void Explosion2(Vector2 from, float explo_range)
    {
        Sequence sequence = DOTween.Sequence();
        sequence.Append(rect.DOJumpAnchorPos(from + Random.insideUnitCircle * explo_range,130,1, 0.5f).SetEase(Ease.OutCubic));
        sequence.Append(rect.GetComponent<Image>().DOFade(0, 0.5f));
        sequence.AppendCallback(() =>
        {
            gameObject.SetActive(false);
            Destroy(gameObject);
        });
    }

    public void Coin(Vector2 from, float dis)
    {
        rect.position = from;
        Sequence seq = DOTween.Sequence()
        .Append(rect.GetComponent<Text>().DOFade(1, 0.5f))
        .Join(rect.DOAnchorPosX(dis, 0.7f))
        .Append(rect.GetComponent<Text>().DOFade(0, 0.5f))
        .AppendCallback(() =>
        {
            gameObject.SetActive(false);
            Destroy(gameObject);
        });
    }
}