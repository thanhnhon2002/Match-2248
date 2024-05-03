using Firebase.Extensions;
using Firebase.RemoteConfig;
using System.Collections.Generic;
using UnityEngine;

public class RemoteConfig : MonoBehaviour
{
    [SerializeField] private bool _showDebug = false;
    [SerializeField] private float _timeBetweenAds = 30f;
    public float TimeBetweenAds => _timeBetweenAds;

    public void InitializeRemoteConfig()
    {
        var defaults = new Dictionary<string, object>
        {
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
                if (_showDebug) Debug.Log("Remote config fetched and activated.");   

                var currentInterval = FirebaseRemoteConfig.DefaultInstance.GetValue("show_interstitial_ads_interval").DoubleValue;
                if (_showDebug) Debug.Log("Current interstitial ads interval: " + currentInterval);
                _timeBetweenAds = (float)currentInterval;
            }
            else
            {
                if (_showDebug) Debug.LogError("Failed to fetch remote config.");
            }
        });
    }
}