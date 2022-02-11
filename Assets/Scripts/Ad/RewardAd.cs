using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Advertisements;
using UnityEngine.UI;

public class RewardAd : MonoBehaviour,IUnityAdsLoadListener,IUnityAdsShowListener
{
    [SerializeField] Button showAdButton;
    [SerializeField] string androidUnitId = "Rewarded_Android";
    //[SerializeField] string iOSUnitId = "Rewarded_iOS";
    string adUnitId = null; //This will remain null for unsupported platforms

    void Awake()
    {
        showAdButton.onClick.AddListener(ShowAd);
    }

    void Start()
    {
#if UNITY_IOS
        //adUnitId = iOSUnitId;
#elif UNITY_ANDROID
        adUnitId = androidUnitId;
#endif
        //안드, ios 외의 플랫폼이면 버튼 안눌리게 처리
        showAdButton.interactable = false;

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
        Advertisement.Load(adUnitId, this);
    }

    public void OnUnityAdsAdLoaded(string placementId)
    {
        Debug.Log("보상형 광고 로드 완료");
        showAdButton.interactable = true;
    }

    public void ShowAd()
    {
        showAdButton.interactable = false;
        Advertisement.Show(adUnitId, this);
    }

    public void OnUnityAdsFailedToLoad(string placementId, UnityAdsLoadError error, string message)
    {
        Debug.Log($"광고 로드 실패: {error.ToString()}-{message}");

    }

    public void OnUnityAdsShowComplete(string placementId, UnityAdsShowCompletionState showCompletionState)
    {
        if(adUnitId==placementId && showCompletionState.Equals(UnityAdsCompletionState.COMPLETED))
        {
            //리워드 구현
            showAdButton.interactable = true;
        }
    }

    public void OnUnityAdsShowFailure(string placementId, UnityAdsShowError error, string message)
    {
        Debug.Log($"Error : {error.ToString()}-{message}");
    }

    public void OnUnityAdsShowClick(string placementId) { }

    public void OnUnityAdsShowStart(string placementId) { }

    /*void OnDestroy()
    {
        showAdButton.onClick.RemoveAllListeners();
    }*/
}
