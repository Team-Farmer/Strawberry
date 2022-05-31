using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;

public class Intro : MonoBehaviour
{
    [Header("[ Intro ]")]
    public GameObject[] introObject=new GameObject[4];
    public RectTransform rect;
    public RectTransform rect2;
    public static Intro instance;
    Sequence sequence;

    void Awake()
    {
        if (Intro.instance == null)
            Intro.instance = this;
    }
    // Use this for initialization
    void Start()
    {
        StartCoroutine(DoScale());
    }


    IEnumerator DoScale()
    {
        sequence = DOTween.Sequence();
        sequence.Append(rect.transform.DOScale(new Vector2(25,25), 1.2f).SetEase(Ease.InCubic));
        sequence.AppendCallback(() => {
            introObject[0].SetActive(false);
        });
        sequence.Append(rect.GetComponent<Image>().DOFade(0, 1.2f));
        sequence.AppendCallback(() => {
            GameManager.instance.EnableObjColliderAll();
            introObject[2].SetActive(false);
        });
        yield return null;
    }




}
