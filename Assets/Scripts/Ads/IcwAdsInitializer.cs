using UnityEngine;
using UnityEngine.Advertisements;

public class IcwAdsInitializer : MonoBehaviour, IUnityAdsInitializationListener
{
    [SerializeField] string _androidGameId = "4879795";
    [SerializeField] bool _testMode = false;

    void Awake()
    {
        InitializeAds();

    }

    public void InitializeAds()
    {
        if (Application.platform != RuntimePlatform.Android && Application.platform != RuntimePlatform.WindowsEditor) return;
        if (!Advertisement.isInitialized) Advertisement.Initialize(_androidGameId, _testMode, this);
        if (Advertisement.isSupported) Debug.LogWarning("Unity Ads support."); else Debug.LogWarning("Unity Ads dont support.");
        //if (Advertisement.isInitialized) Debug.LogWarning("Unity Ads initalized."); else Debug.LogWarning("Unity Ads dont initalized.");
        
    }

    public void OnInitializationComplete()
    {
        Debug.LogWarning("Unity Ads initialization complete.");
    }

    public void OnInitializationFailed(UnityAdsInitializationError error, string message)
    {
        Debug.LogWarning($"Unity Ads Initialization Failed: {error.ToString()} - {message}");
    }
}