using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AdHeart : MonoBehaviour
{
    [SerializeField] Button adHeartBtn;
    [SerializeField] PanelAnimation panel;
    [SerializeField] GameObject panelBlack;
    public Text adHeartText;
    public Text remainAdText;

    void Awake()
    {
        GameManager.instance.OnOnline += HeartAdCountCheck;
        GameManager.instance.OnOffline += AdBtnOff;
    }

    public void HeartAdCountCheck()
    {
        if (DataController.instance.gameData.heartAdCnt > 0)
        {
            adHeartText.text = "광고를 시청하고\n하트 " + (5 * (DataController.instance.gameData.researchLevelAv + 1)) + "개를 받을까요?";
        }
        else
        {
            adHeartText.text = "오늘 볼 수 있는 광고를\n모두 시청하였어요!";
            adHeartBtn.interactable = false;
        }
        remainAdText.text = $"오늘 남은 횟수 : {DataController.instance.gameData.heartAdCnt}";
    }

    public void AdBtnOff()
    {
        adHeartBtn.interactable = false;
    }


    public void OnClickPlusHeartBtn()
    {
        RewardAd.instance.OnAdComplete += ReceiveHeart;
        RewardAd.instance.OnAdFailed += OnFailAd;
        RewardAd.instance.ShowAd();
        adHeartBtn.interactable = false;
    }

    void ReceiveHeart()
    {
        GameManager.instance.GetHeart(5 * (DataController.instance.gameData.researchLevelAv + 1));
        adHeartBtn.interactable = true;
        RewardAd.instance.OnAdComplete -= ReceiveHeart;
        RewardAd.instance.OnAdFailed -= OnFailAd;

        DataController.instance.gameData.heartAdCnt--;
        panel.gameObject.SetActive(false); // 시원 수정
        panelBlack.SetActive(false);
    }

    void OnFailAd()
    {
        RewardAd.instance.OnAdComplete -= ReceiveHeart;
        RewardAd.instance.OnAdFailed -= OnFailAd;
        adHeartBtn.interactable = true;
    }
}
