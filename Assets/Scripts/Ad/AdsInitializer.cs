using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Advertisements;

public class AdsInitializer : MonoBehaviour,IUnityAdsInitializationListener
{
    public static AdsInitializer instance = null;

    [SerializeField] string androidGameId;
    [SerializeField] string iosGameId;
    [SerializeField] bool isTestMode = true;
    string gameId;

    void Awake()
    {
        InitializeAds();
    }

    void Start()
    {
        //Singleton
        if(instance==null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else if (instance != this)
        {
            Destroy(gameObject);
        }
    }

    public void InitializeAds()
    {
        //gameId = (Application.platform == RuntimePlatform.Android) ? androidGameId : iosGameId;
        gameId = androidGameId;
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
