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

    void Update()
    {
        adHeartText.text = "광고를 시청하고\n하트 " + (10 * (DataController.instance.gameData.researchLevelAv + 1)) + "개를 받을까요?";
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
        GameManager.instance.GetHeart(10 * (DataController.instance.gameData.researchLevelAv + 1));
        adHeartBtn.interactable = true;
        RewardAd.instance.OnAdComplete -= ReceiveHeart;
        RewardAd.instance.OnAdFailed -= OnFailAd;

        panel.CloseScale();
        panelBlack.SetActive(false);
    }

    void OnFailAd()
    {
        RewardAd.instance.OnAdComplete -= ReceiveHeart;
        RewardAd.instance.OnAdFailed -= OnFailAd;
        adHeartBtn.interactable = true;
    }
}
