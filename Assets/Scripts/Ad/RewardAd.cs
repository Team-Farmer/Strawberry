using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Advertisements;
using UnityEngine.UI;

public class RewardAd : MonoBehaviour, IUnityAdsLoadListener, IUnityAdsShowListener
{
    [SerializeField] Button showAdButton;
    [SerializeField] string androidUnitId = "Rewarded_Android";
    //[SerializeField] string iOSUnitId = "Rewarded_iOS";
    public string adUnitId = null; //This will remain null for unsupported platforms

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
        Advertisement.Load(adUnitId,this);
    }

    public void OnUnityAdsAdLoaded(string placementId)
    {
        Debug.Log("보상형 광고 로드 완료");
        showAdButton.interactable = true;
    }

    public void ShowAd()
    {
        showAdButton.interactable = false;
        Advertisement.Show(adUnitId,this);

        //임시조치 : UnityAds 4.0문제
        //다음에 고치도록 하겠습니당 220610
        //220810 UnityAds 4.2로 변경
    }


    public void OnUnityAdsFailedToLoad(string placementId, UnityAdsLoadError error, string message)
    {
        Debug.Log($"광고 로드 실패: {error}-{message}");
    }


    public void OnUnityAdsShowComplete(string placementId, UnityAdsShowCompletionState showCompletionState)
    {
        Debug.Log("OnUnityAdsShowComplete "+showCompletionState);
        Debug.Log(UnityAdsCompletionState.COMPLETED.Equals(showCompletionState));
        //if (adUnitId.Equals(placementId) && showCompletionState.Equals(UnityAdsCompletionState.COMPLETED))
        if(adUnitId.Equals(placementId)/*&&UnityAdsCompletionState.COMPLETED.Equals(showCompletionState)*/) //뒷쪽 코드가 계속 false가 뜨네...왜이러지 220810
        {
            //리워드 구현 - 다른보상 추가되면 코드 수정해야 함
            showAdButton.interactable = true;

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
}
