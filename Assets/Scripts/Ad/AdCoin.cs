using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AdCoin : MonoBehaviour
{
    [SerializeField] Button adCoinBtn;
    [SerializeField] PanelAnimation panel;
    [SerializeField] GameObject panelBlack;

    public void OnClickPlusCoinBtn()
    {
        RewardAd.instance.OnAdComplete += ReceiveCoin;
        RewardAd.instance.OnAdFailed += OnAdFail;
        RewardAd.instance.ShowAd();
        adCoinBtn.interactable = false;
    }

    void ReceiveCoin()
    {
        GameManager.instance.GetCoin(1000);
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
