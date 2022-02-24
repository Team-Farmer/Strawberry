using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;

public class PanelAnimation : MonoBehaviour
{
    public int UpPosition;
    public int DownPosition;

    public void OpenUP()
    {
        RectTransform rt = GetComponent<RectTransform>();
        gameObject.SetActive(true);

        rt.DOAnchorPosY(UpPosition, 0.3f);
    }

    public void CloseUp()
    {
        RectTransform rt = GetComponent<RectTransform>();
        rt.DOAnchorPosY(-1940, 0.2f);
        Invoke("ActiveOff", 0.3f);
    }

    public void OpenDown()
    {
        RectTransform rt = GetComponent<RectTransform>();
        gameObject.SetActive(true);

        rt.DOAnchorPosY(DownPosition, 0.3f);
    }

    public void CloseDown()
    {
        RectTransform rt = GetComponent<RectTransform>();
        rt.DOAnchorPosY(1870, 0.3f);
        Invoke("ActiveOff", 0.3f);
    }

    public void FadeOut()
    {
        gameObject.GetComponent<Image>().DOFade(0.0f, 0.3f);
        Invoke("ActiveOff", 0.3f);
    }

    public void Fadein()
    {
        gameObject.GetComponent<Image>().DOFade(0.5f, 0.3f);
    }

    public void ActiveOff()
    {
        gameObject.SetActive(false);
    }

    public void OpenScale()
    {
        RectTransform rt = GetComponent<RectTransform>();
        gameObject.SetActive(true);
        rt.transform.DOScale(new Vector3(1f, 1f, 1f), 0.3f);

    }

    public void CloseScale()
    {
        RectTransform rt = GetComponent<RectTransform>();
        rt.transform.DOScale(new Vector3(0f, 0f, 1f), 0.3f);
        Invoke("ActiveOff", 0.2f);
    }
}
