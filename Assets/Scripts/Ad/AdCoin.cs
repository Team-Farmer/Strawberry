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

    void Update()
    {
        adCoinText.text = "광고를 시청하고\n코인 "+ (1000 * (DataController.instance.gameData.researchLevelAv + 1)) + "A를 받을까요?";
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
        GameManager.instance.GetCoin(1000 * (DataController.instance.gameData.researchLevelAv + 1));
        RewardAd.instance.OnAdComplete -= ReceiveCoin;
        RewardAd.instance.OnAdFailed -= OnAdFail;
        adCoinBtn.interactable = true;

        panelBlack.SetActive(false);
        panel.CloseScale();
    }

    void OnAdFail()
    {
        RewardAd.instance.OnAdFailed -= OnAdFail;
        RewardAd.instance.OnAdComplete -= OnClickPlusCoinBtn;
        adCoinBtn.interactable = true;
    }
}
