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
    [SerializeField] private float _giftInterval = 120f;
    [SerializeField] private float _minSessionTimeShowAds = 60f;
    [SerializeField] private float _minMinuteSpecialOffer = 2f;
    [SerializeField] private float _loadingTime = 2.5f;
    [SerializeField] private float _timePowerReward = 180f;
    public bool COLLAPSIBLE_BANNER_ENABLED => _collapsibleBannerEnabled;
    public bool COLLAPSIBLE_FALLBACK_ENABLED => _collapsibleBannerFallbackEnabled;
    public float COLLAPSIBLE_BANNER_INTERVAL => _collapsibleBannerInterval;
    public float TIME_BETWEEN_ADS => _timeBetweenAds;
    public float MIN_SESSION_TIME_SHOW_ADS => _minSessionTimeShowAds;
    public float GIFT_INTERVAL => _giftInterval;
    public float MIN_MINUTE_SPECIAL_OFFER => _minMinuteSpecialOffer;
    public float LOADING_TIME => _loadingTime;
    public float TIME_POWER_REWARD => _timePowerReward;

    public bool fetch { get; private set; }

    public void InitializeRemoteConfig()
    {
        fetch = false;
        var defaults = new Dictionary<string, object>
        {
            { "collapsible_banner_enabled", _collapsibleBannerEnabled},
            { "collapsible_banner_fallback_enabled", _collapsibleBannerFallbackEnabled},
            { "collapsible_banner_interval", _collapsibleBannerInterval},
            { "show_interstitial_ads_interval", _timeBetweenAds},
            { "gif_interval", _giftInterval },
            { "min_session_time_show_ads", _minSessionTimeShowAds},
            { "min_minute_special_offer", _minMinuteSpecialOffer},
            { "loading_time", _loadingTime},
            { "time_power_reward", _timePowerReward},
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
                var currentMinSessionTimeShowAds = FirebaseRemoteConfig.DefaultInstance.GetValue("min_session_time_show_ads").DoubleValue;
                var gifInterval = FirebaseRemoteConfig.DefaultInstance.GetValue("gif_interval").DoubleValue;
                var specialOfferInterval = FirebaseRemoteConfig.DefaultInstance.GetValue("min_minute_special_offer").DoubleValue;
                var loadingTime  = FirebaseRemoteConfig.DefaultInstance.GetValue("loading_time").DoubleValue;
                var powerReward  = FirebaseRemoteConfig.DefaultInstance.GetValue("time_power_reward").DoubleValue;
                if (_showDebug)
                {
                    Debug.Log("Current banner enabled: " + currentBannerEnabled);
                    Debug.Log("Current banner fallback enabled: " + currentBannerFallbackEnabled);
                    Debug.Log("Current banner interval: " + currentBannerInterval);
                    Debug.Log("Current interstitial ads interval: " + currentInterval);
                    Debug.Log("Current min session time show ads: " + currentMinSessionTimeShowAds);
                }
                _collapsibleBannerEnabled = currentBannerEnabled;
                _collapsibleBannerFallbackEnabled = currentBannerFallbackEnabled;
                _collapsibleBannerInterval = (float)currentBannerInterval;
                _timeBetweenAds = (float)currentInterval;
                _minSessionTimeShowAds = (float)currentMinSessionTimeShowAds;
                _giftInterval = (float)gifInterval;
                _minMinuteSpecialOffer = (float)specialOfferInterval;
                _loadingTime = (float)loadingTime;
                _timePowerReward = (float)powerReward;
                fetch = true;
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