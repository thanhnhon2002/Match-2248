using Firebase.Extensions;
using Firebase.RemoteConfig;
using System.Collections.Generic;
using UnityEngine;

public class RemoteConfig : MonoBehaviour
{
    [SerializeField] private bool _showDebug = false;
    [SerializeField] private bool _collapsibleBannerEnabled = true;
    [SerializeField] private bool _collapsibleBannerFallbackEnabled = true;
    [SerializeField] private float _collapsibleBannerInterval = 40f;
    [SerializeField] private float _timeBetweenAds = 120f;
    public bool COLLAPSIBLE_BANNER_ENABLED => _collapsibleBannerEnabled;
    public bool COLLAPSIBLE_FALLBACK_ENABLED => _collapsibleBannerFallbackEnabled;
    public float COLLAPSIBLE_BANNER_INTERVAL => _collapsibleBannerInterval;
    public float TIME_BETWEEN_ADS => _timeBetweenAds;

    public void InitializeRemoteConfig()
    {
        var defaults = new Dictionary<string, object>
        {
            { "collapsible_banner_enabled", _collapsibleBannerEnabled},
            { "collapsible_banner_fallback_enabled", _collapsibleBannerFallbackEnabled},
            { "collapsible_banner_interval", _collapsibleBannerInterval},
            { "show_interstitial_ads_interval", _timeBetweenAds},
        };
        FirebaseRemoteConfig.DefaultInstance.SetDefaultsAsync(defaults).ContinueWithOnMainThread(task =>
        {
            if (_showDebug) Debug.Log("Defaults set.");
            FetchRemoteConfig();
        });
    }
    private void FetchRemoteConfig()
    {
        FirebaseRemoteConfig.DefaultInstance.FetchAndActivateAsync().ContinueWithOnMainThread(task =>
        {
            if (task.IsCompleted && !task.IsFaulted && !task.IsCanceled)
            {
                if (_showDebug)
                {
                    Debug.Log("Remote config fetched and activated.");
                }

                var currentBannerEnabled = FirebaseRemoteConfig.DefaultInstance.GetValue("collapsible_banner_enabled").BooleanValue;
                var currentBannerFallbackEnabled = FirebaseRemoteConfig.DefaultInstance.GetValue("collapsible_banner_fallback_enabled").BooleanValue;
                var currentBannerInterval = FirebaseRemoteConfig.DefaultInstance.GetValue("collapsible_banner_interval").DoubleValue;
                var currentInterval = FirebaseRemoteConfig.DefaultInstance.GetValue("show_interstitial_ads_interval").DoubleValue;
                if (_showDebug)
                {
                    Debug.Log("Current banner enabled: " + currentBannerEnabled);
                    Debug.Log("Current banner fallback enabled: " + currentBannerFallbackEnabled);
                    Debug.Log("Current banner interval: " + currentBannerInterval);
                    Debug.Log("Current interstitial ads interval: " + currentInterval);
                }
                _collapsibleBannerEnabled = currentBannerEnabled;
                _collapsibleBannerFallbackEnabled = currentBannerFallbackEnabled;
                _collapsibleBannerInterval = (float)currentBannerInterval;
                _timeBetweenAds = (float)currentInterval;
            }
            else
            {
                if (_showDebug)
                {
                    Debug.LogError("Failed to fetch remote config.");
                }
            }
        });
    }
}