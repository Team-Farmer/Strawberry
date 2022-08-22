using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Advertisements;
using UnityEngine.UI;

public class RewardAd : MonoBehaviour, IUnityAdsLoadListener, IUnityAdsShowListener
{
    [SerializeField] Button[] showAdButtons;
    [SerializeField] string androidUnitId = "Rewarded_Android";
    //[SerializeField] string iOSUnitId = "Rewarded_iOS";
    string adUnitId = null; //This will remain null for unsupported platforms

    void Start()
    {
#if UNITY_IOS
        //adUnitId = iOSUnitId;
#elif UNITY_ANDROID
        adUnitId = androidUnitId;
#endif

        StartCoroutine(LoadAd());
    }


    //Load content to the Ad Unit
    IEnumerator LoadAd()
    {
        WaitForSeconds wait = new WaitForSeconds(.5f);
        while (!Advertisement.isInitialized)
        {
            //광고 로드중 팝업 띄우기
            yield return wait;
        }
        Advertisement.Load(adUnitId,this);
    }

    public void OnUnityAdsAdLoaded(string placementId)
    {
        Debug.Log("보상형 광고 로드 완료");
        InteractableBtn(true);
    }

    public void ShowAd()
    {
        InteractableBtn(false);
        Advertisement.Show(adUnitId,this);
    }


    public void OnUnityAdsFailedToLoad(string placementId, UnityAdsLoadError error, string message)
    {
        Debug.Log($"광고 로드 실패: {error}-{message}");
    }


    public void OnUnityAdsShowComplete(string placementId, UnityAdsShowCompletionState showCompletionState)
    {
        if(adUnitId.Equals(placementId)&& UnityAdsShowCompletionState.COMPLETED.Equals(showCompletionState))
        {
            ArbeitMgr arbeit = GameObject.FindGameObjectWithTag("Arbeit").GetComponent<ArbeitMgr>();
            float coEffi = arbeit.Pigma();
            float totalCoin = (DataController.instance.gameData.truckCoin
                + GameManager.instance.bonusTruckCoin) * coEffi * 3;
            GameManager.instance.GetCoin((int)totalCoin);

            Debug.Log($"{totalCoin} 획득");
            DataController.instance.gameData.truckBerryCnt = 0;
            DataController.instance.gameData.truckCoin = 0;
            StartCoroutine(LoadAd());//광고로드
        }
    }

    public void OnUnityAdsShowFailure(string placementId, UnityAdsShowError error, string message)
    {
        Debug.Log($"광고 보여주기 Error : {error}-{message}");
    }

    public void OnUnityAdsShowClick(string placementId) { }

    public void OnUnityAdsShowStart(string placementId) { }

    /*void OnDestroy()
    {
        showAdButton.onClick.RemoveAllListeners();
    }*/

    void InteractableBtn(bool state)
    {
        for(int i = 0; i < showAdButtons.Length; i++)
        {
            showAdButtons[i].interactable = state;
        }
    }
}
