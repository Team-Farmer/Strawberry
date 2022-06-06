using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;

public class Intro : MonoBehaviour
{
    [Header("[ Intro ]")]
    public GameObject[] introObject=new GameObject[4];
    public Sprite[] sprites = new Sprite[2];
    public RectTransform[] rect = new RectTransform[2];

    private static Intro instance;
    Sequence sequence;

    void Awake()
    {
        if (Intro.instance == null)
            Intro.instance = this;
    }

    void Start()
    {
        StartCoroutine(DoFade());
    }


    IEnumerator DoScale()
    {
        sequence = DOTween.Sequence();
        sequence.Append(rect[0].transform.DOScale(new Vector2(25,25), 1.2f).SetEase(Ease.InCubic));
        sequence.AppendCallback(() => {
            introObject[0].SetActive(false);
        });
        sequence.Append(rect[0].GetComponent<Image>().DOFade(0, 1.2f));
        sequence.AppendCallback(() => {
            GameManager.instance.EnableObjColliderAll();
            introObject[2].SetActive(false);
        });
        yield return null;
    }

    IEnumerator DoFade()
    {
        introObject[0].SetActive(true);
        introObject[1].SetActive(true);
        sequence = DOTween.Sequence()
        .Append(rect[0].GetComponent<Image>().DOFade(1, 2.0f))
        .AppendCallback(() => {
            introObject[2].GetComponent<Image>().sprite = sprites[0];
        })
        .AppendInterval(1)
        .Append(rect[1].GetComponent<Image>().DOFade(1, 2.0f)).SetEase(Ease.InCubic)
        .AppendCallback(() => {
            introObject[3].SetActive(false);
        })
        .Append(rect[2].GetComponent<Image>().DOFade(0, 1.0f)).SetEase(Ease.OutCubic)
        .AppendCallback(() => {
            introObject[4].SetActive(false);
        });
        yield return null;
    }




}
