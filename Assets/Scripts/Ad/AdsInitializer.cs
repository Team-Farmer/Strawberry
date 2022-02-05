using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Advertisements;

public class AdsInitializer : MonoBehaviour,IUnityAdsInitializationListener
{
    [SerializeField] string androidGameId;
    [SerializeField] string iosGameId;
    [SerializeField] bool isTestMode = true;
    private string gameId;

    void Awake()
    {
        InitializeAds();
    }

    public void InitializeAds()
    {
        gameId = (Application.platform == RuntimePlatform.Android) ? androidGameId : iosGameId;
        Advertisement.Initialize(gameId, isTestMode, this);
    }

    public void OnInitializationComplete()
    {
        Debug.Log("Unity Ads initialization complete");
    }

    public void OnInitializationFailed(UnityAdsInitializationError error, string message)
    {
        Debug.Log($"unity ads initializaion failed : {error.ToString()}-{message}");
    }
}
