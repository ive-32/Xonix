using UnityEngine;
using UnityEngine.Advertisements;
using Assets.Scripts;

public class IcwInterstitialAds : MonoBehaviour, IUnityAdsLoadListener, IUnityAdsShowListener
{           
    [SerializeField] string _androidAdUnitId = "Interstitial_Android";
    private bool isLoaded = false;
    string _adUnitId;

    void Awake()
    {
        if (Application.platform != RuntimePlatform.Android && Application.platform != RuntimePlatform.WindowsEditor) return;
        _adUnitId = _androidAdUnitId;
        if (!Advertisement.isInitialized) IcwObjects.IcwAdsInitializerclass.InitializeAds();
    }

    public void LoadAd()
    {
        isLoaded = false;
        if (!Advertisement.isInitialized) IcwObjects.IcwAdsInitializerclass.InitializeAds();
        Advertisement.Load(_adUnitId, this);
    }

    public void TryToShowAd()
    {
        string startdate = PlayerPrefs.GetString("StartDate");
        if (!isLoaded)
        {
            LoadAd();
            return;
        }

        if (!System.DateTime.TryParse(startdate, out System.DateTime dt))
        {
            PlayerPrefs.SetString("StartDate", System.DateTime.Today.ToString());
            return;
        }
        //PlayerPrefs.SetString("StartDate", (System.DateTime.Today.AddDays(-2)).ToString());
        if ((System.DateTime.Today - dt).TotalDays < 2 || UnityEngine.Random.Range(0, 100) > 10) return;
        ShowAd();
    }


    public void ShowAd()
    {
        if (!isLoaded)
        {
            LoadAd();
            return;
        }
        Advertisement.Show(_adUnitId, this);
    }

    // Implement Load Listener and Show Listener interface methods: 
    public void OnUnityAdsAdLoaded(string adUnitId)
    {
        isLoaded = true;
    }

    public void OnUnityAdsFailedToLoad(string adUnitId, UnityAdsLoadError error, string message)
    {
        Debug.LogWarning($"Error loading Ad Unit: {adUnitId} - {error.ToString()} - {message}");
        LoadAd();
    }

    public void OnUnityAdsShowFailure(string adUnitId, UnityAdsShowError error, string message)
    {
        Debug.LogWarning($"Error showing Ad Unit {adUnitId}: {error.ToString()} - {message}");
        LoadAd();
    }

    public void OnUnityAdsShowStart(string adUnitId) { }
    public void OnUnityAdsShowClick(string adUnitId) { }
    public void OnUnityAdsShowComplete(string adUnitId, UnityAdsShowCompletionState showCompletionState) 
    {
        LoadAd();
    }
}
