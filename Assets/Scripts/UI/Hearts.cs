using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;


public class Hearts : MonoBehaviour
{
    public RectTransform[] heart = new RectTransform[5];
    Vector2 vec = new Vector2(-125, 980);

    void Start()
    {

        for (int i = 0; i < 3; i++)
        {
            StartCoroutine(WaitForIt(i));
        }

        StartCoroutine(ActiveOff());
    }

    IEnumerator ActiveOff()
    {
        yield return new WaitForSeconds(5.0f);
        gameObject.SetActive(false);
    }

    IEnumerator WaitForIt(int i)
    {
        yield return new WaitForSeconds(2.0f);
        heart[i].DOJumpAnchorPos(vec, 50f, 0, 1f).SetEase(Ease.InQuad);
    }
}
