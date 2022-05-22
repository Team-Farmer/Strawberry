using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;
public class HeartAcquireFx : MonoBehaviour
{
    public RectTransform rect;

    public void Explosion(Vector2 from, Vector2 to, float explo_range)
    {
        rect.position = from;
        Sequence sequence = DOTween.Sequence();
        sequence.Append(rect.DOAnchorPos(from + Random.insideUnitCircle * explo_range,0.25f).SetEase(Ease.OutCubic));
        sequence.Append(rect.DOAnchorPos(to, 0.5f).SetEase(Ease.InCubic));
        sequence.AppendCallback(() => { gameObject.SetActive(false);
            Destroy(gameObject);
        });
    }

    public void Explosion2(Vector2 from, float explo_range)
    {
        Sequence sequence = DOTween.Sequence();
        sequence.Append(rect.DOAnchorPos(from + Random.insideUnitCircle * explo_range, 0.25f).SetEase(Ease.OutCubic));
        sequence.Append(rect.DOAnchorPos(from, 0.5f).SetEase(Ease.InCubic));
        sequence.AppendCallback(() => {
            gameObject.SetActive(false);
            Destroy(gameObject);
        });
    }
}