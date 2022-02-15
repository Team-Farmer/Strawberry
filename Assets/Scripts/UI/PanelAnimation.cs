using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class PanelAnimation : MonoBehaviour
{

    public int UpPosition;
    public void Open()
    {
        RectTransform rt = GetComponent<RectTransform>();
        gameObject.SetActive(true);

        rt.DOAnchorPosY(UpPosition, 0.2f);
    }

    public void Close()
    {
        RectTransform rt = GetComponent<RectTransform>();
        rt.DOAnchorPosY(-1940, 0.2f);
        Invoke("ActiveOff", 0.2f);
    }

    public void ActiveOff()
    {
        gameObject.SetActive(false);
    }
}
