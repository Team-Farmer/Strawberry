using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AdCoin : MonoBehaviour
{
    [SerializeField] Button adCoinBtn;
    [SerializeField] PanelAnimation panel;
    [SerializeField] GameObject panelBlack;
    public Text adCoinText;
    public Text remainAdText;

    void Awake()
    {
        GameManager.instance.OnOnline += CoinAdCountCheck;
        GameManager.instance.OnOffline += AdBtnOff;
    }

    public void CoinAdCountCheck()
    {
        if (DataController.instance.gameData.coinAdCnt > 0)
        {
            adCoinText.text = "광고를 시청하고\n코인 " + (500 * (DataController.instance.gameData.researchLevelAv + 1)) + "A를 받을까요?";
        }
        else
        {
            adCoinText.text = "오늘 볼 수 있는 광고를\n모두 시청하였어요!";
            adCoinBtn.interactable = false;
        }
        remainAdText.text = $"오늘 남은 횟수 : {DataController.instance.gameData.coinAdCnt}";
    }

    public void AdBtnOff()
    {
        adCoinBtn.interactable = false;
    }

    public void OnClickPlusCoinBtn()
    {
        RewardAd.instance.OnAdComplete += ReceiveCoin;
        RewardAd.instance.OnAdFailed += OnAdFail;
        RewardAd.instance.ShowAd();
        adCoinBtn.interactable = false;
    }

    void ReceiveCoin()
    {
        GameManager.instance.GetCoin(500 * (DataController.instance.gameData.researchLevelAv + 1));
        RewardAd.instance.OnAdComplete -= ReceiveCoin;
        RewardAd.instance.OnAdFailed -= OnAdFail;
        adCoinBtn.interactable = true;

        DataController.instance.gameData.coinAdCnt--;
        panelBlack.SetActive(false);
        panel.gameObject.SetActive(false);// 시원 수정
    }

    void OnAdFail()
    {
        Debug.Log("실패");
        RewardAd.instance.OnAdFailed -= OnAdFail;
        RewardAd.instance.OnAdComplete -= OnClickPlusCoinBtn;
        adCoinBtn.interactable = true;
    }
}
