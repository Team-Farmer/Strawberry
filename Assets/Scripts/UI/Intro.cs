using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;

public class Intro : MonoBehaviour
{
    [Header("[ Intro ]")]
    public GameObject[] introObject = new GameObject[4];
    public Sprite[] sprites = new Sprite[2];
    public RectTransform rect;

    //튜토리얼 및 배너광고
    public GameObject tutorialObject;
    public BannerAd bannerAd;

    static public bool isEnd=false;

    private static Intro instance;
    Sequence sequence;

    void Awake()
    {
        if (Intro.instance == null)
            Intro.instance = this;

        if (GooglePlayManager.Instance != null)
            GooglePlayManager.Instance.UpdateApp();

        isEnd = false;
    }

    void Start()
    {
        StartCoroutine(DoScale());
        introObject[5].SetActive(false);

        if (!DataController.instance.gameData.isTutorialDone)
        {
            tutorialObject.SetActive(true);
        }
    }

    IEnumerator DoScale()
    {
        sequence = DOTween.Sequence();
        sequence.Append(rect.DOAnchorPos(new Vector3(157, -313, 0), 0.7f))
                .Join(rect.transform.DOScale(Vector3.one, 0.7f))
                .AppendCallback(() =>
                {
                    AudioManager.instance.WinkAudioPlay();
                })
                .AppendInterval(0.2f)
                .AppendCallback(() =>
                {
                    introObject[0].GetComponent<Image>().sprite = sprites[0];

                })
                .AppendInterval(0.4f)
                .Append(rect.transform.DOScale(Vector3.one * 0.002f, 0.4f))
                .AppendInterval(0.1f)
                .AppendCallback(() =>
                {

                    introObject[1].SetActive(false);
                    introObject[2].SetActive(true);
                    introObject[3].SetActive(true);
                })
                .AppendInterval(0.4f)
                .Append(rect.transform.DOScale(Vector3.one * 7f, 0.7f).SetEase(Ease.InCubic))
                .AppendCallback(() =>
                {

                    introObject[4].SetActive(false);

                    //예림
                    if (DataController.instance.gameData.isTutorialDone)
                    {
                        AudioManager.instance.ResumePlayAudio("RainSFXSound");
                        GameManager.instance.EnableObjColliderAll();
                        bannerAd.ShowBanner(); // 광고 끄고싶을 때 주석하기
                    }
                });

        yield return new WaitForSeconds(1.8f);
        isEnd = true;
        GameManager.instance.StartPrework();
        //Debug.Log(DataController.instance.gameData.lastExitTime);
    }
}
