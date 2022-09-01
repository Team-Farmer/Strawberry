using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Advertisements;

public class BannerAd : MonoBehaviour
{
    [SerializeField] BannerPosition bannerPosition = BannerPosition.TOP_CENTER;

    [SerializeField] string androidAdUnitId = "Banner_Android";
    // [SerializeField] string iosAdUnitId = "Banner_iOS"; // 콘솔창에서 미사용 변수라고해서 주석처리 해둘게요! -희규
    string adUnitId = null; //This will remain null for unsupported platforms.

    void Start()
    {
        //Get the Ad Unit Id for the current platform
#if UNITY_IOS
        adUnitId=iosAdUnitId;
#elif UNITY_ANDROID
        adUnitId = androidAdUnitId;
#endif

        //초기화가 됐는지 확인하고 배너광고 로드
        StartCoroutine(CheckInitialize());
    }

    IEnumerator CheckInitialize()
    {
        WaitForSeconds wait = new WaitForSeconds(.5f);
        while (!Advertisement.isInitialized)
        {
            yield return wait;
        }
        Advertisement.Banner.SetPosition(bannerPosition);
        LoadBanner();
    }

    public void LoadBanner()
    {
        //Set up options to notify the SDK of load events
        BannerLoadOptions options = new BannerLoadOptions
        {
            loadCallback = OnBannerLoaded,
            errorCallback = OnBannerError
        };

        //Load the Ad Unit with banner content
        Advertisement.Banner.Load(adUnitId, options);
    }

    void OnBannerLoaded()
    {
        Debug.Log("배너광고 로드");
        Advertisement.Banner.Show(adUnitId);
    }

    void OnBannerError(string message)
    {
        Debug.Log($"배너광고 로드 실패:{message}");
    }

    /*void OnDestroy()
    {
        if (Advertisement.Banner.isLoaded)
        {
            Advertisement.Banner.Hide(true);
        }    
    }*/
}
