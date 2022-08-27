using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Advertisements;
using System;

public class RewardAd : MonoBehaviour, IUnityAdsLoadListener, IUnityAdsShowListener
{
    public static RewardAd instance = null;
    public GameObject Popup;
    public Action OnAdComplete;
    public Action OnAdFailed;

    [SerializeField] string androidUnitId = "Rewarded_Android";
    //[SerializeField] string iOSUnitId = "Rewarded_iOS";
    string adUnitId = null; //This will remain null for unsupported platforms

    void Awake()
    {
        if (instance == null)
            instance = this;
    }

    void Start()
    {
#if UNITY_IOS
        //adUnitId = iOSUnitId;
#elif UNITY_ANDROID
        adUnitId = androidUnitId;
#endif

    }


    //Load content to the Ad Unit
    public void LoadAd()
    {
        //while (!Advertisement.isInitialized)
        //{
        ////광고 로드중 팝업 띄우기
        //yield return wait;
        //}
        if (Advertisement.isInitialized)
            Advertisement.Load(adUnitId, this);
        else
            Debug.Log("잠시 후 다시 시도해주세요");
    }

    public void OnUnityAdsAdLoaded(string placementId)
    {
        Debug.Log("보상형 광고 로드 완료");
    }

    public void ShowAd()
    {
        Advertisement.Show(adUnitId, this);
    }


    public void OnUnityAdsFailedToLoad(string placementId, UnityAdsLoadError error, string message)
    {
        Debug.Log($"광고 로드 실패: {error}-{message}");

        OnAdFailed();

        //잠시후 다시 시도해주세요 팝업
        //Popup.SetActive(true);
        Debug.Log("잠시후 다시 시도해주세요");
    }


    public void OnUnityAdsShowComplete(string placementId, UnityAdsShowCompletionState showCompletionState)
    {
        if (adUnitId.Equals(placementId) && UnityAdsShowCompletionState.COMPLETED.Equals(showCompletionState))
        {
            if (OnAdComplete != null)
                OnAdComplete();
        }
    }

    public void OnUnityAdsShowFailure(string placementId, UnityAdsShowError error, string message)
    {
        Debug.Log($"광고 보여주기 Error : {error}-{message}");
    }

    public void OnUnityAdsShowClick(string placementId) { }

    public void OnUnityAdsShowStart(string placementId) { }

}