using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Firebase.Extensions;
using Firebase;
using Firebase.Analytics;
using DarkcupGames;
using UnityEngine.SceneManagement;
using System;
using Firebase.Crashlytics;

public enum AnalyticsEvent
{
    level_start, level_passed, level_failed, will_show_interstitial, will_show_rewarded, ui_appear, button_click
}

public enum UserPopertyKey
{
    level_max, last_level, last_placement, total_interstitial_ads, total_rewarded_ads
}

public class FirebaseManager : MonoBehaviour
{
    public static FirebaseManager Instance;
    public FirebaseApp app;
    public static RemoteConfig remoteConfig { get; private set; }
    public bool ready { get; private set; }

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            remoteConfig = GetComponentInChildren<RemoteConfig>();
        } else
        {
            Destroy(gameObject);
            return;
        }
    }

    IEnumerator Start()
    {
        yield return FirebaseApp.CheckAndFixDependenciesAsync().ContinueWithOnMainThread(task =>
        {
            var dependencyStatus = task.Result;
            if (dependencyStatus == DependencyStatus.Available)
            {
                Crashlytics.ReportUncaughtExceptionsAsFatal = true;
                app = FirebaseApp.DefaultInstance;
                remoteConfig.InitializeRemoteConfig();
                ready = true;
                Debug.Log("Firebas is ready");
            } else
            {
                Debug.LogError(System.String.Format(
                  "Could not resolve all Firebase dependencies: {0}", dependencyStatus));
            }
        });
    }

    public void LogLevelStart(int level, bool restart)
    {
        if (!ready) return;
        var param1 = new Parameter("level", level);
        var param2 = new Parameter("restart", restart.ToString());
        FirebaseAnalytics.LogEvent(AnalyticsEvent.level_start.ToString(), param1, param2);
    }

    public void LogLevelPass(int level, float time)
    {
        if (!ready) return;
        var param1 = new Parameter("level", level);
        var param2 = new Parameter("time_spent", time);
        FirebaseAnalytics.LogEvent(AnalyticsEvent.level_passed.ToString(), param1, param2);
    }

    public void LogLevelFail(int level, float time)
    {
        if (!ready) return;
        var param1 = new Parameter("level", level);
        var param2 = new Parameter("time_spent", time);
        FirebaseAnalytics.LogEvent(AnalyticsEvent.level_failed.ToString(), param1, param2);
    }

    public void LogIntertisial(string placement)
    {
        if (!ready) return;
        var param1 = new Parameter("internet_available", Application.internetReachability.ToString());
        var param2 = new Parameter("placement", placement);
        var param3 = new Parameter("has_ads", MaxMediationManager.intertistial.IsAdAvailable().ToString());
        FirebaseAnalytics.LogEvent(AnalyticsEvent.will_show_interstitial.ToString(), param1, param2, param3);
    }

    public void LogReward(string placement)
    {
        if (!ready) return;
        var param1 = new Parameter("internet_available", Application.internetReachability.ToString());
        var param2 = new Parameter("placement", placement);
        var param3 = new Parameter("has_ads", MaxMediationManager.rewarded.IsAdAvailable().ToString());
        FirebaseAnalytics.LogEvent(AnalyticsEvent.will_show_rewarded.ToString(), param1, param2, param3);
    }

    public void LogUIAppear(string name)
    {
        if (!ready) return;
        var param1 = new Parameter("screen_name", SceneManager.GetActiveScene().name);
        var param2 = new Parameter("name", name);
        FirebaseAnalytics.LogEvent(AnalyticsEvent.ui_appear.ToString(), param1, param2);
    }

    public void LogButtonClick(string name)
    {
        if (!ready) return;
        var param1 = new Parameter("screen_name", SceneManager.GetActiveScene().name);
        var param2 = new Parameter("name", name);
        FirebaseAnalytics.LogEvent(AnalyticsEvent.button_click.ToString(), param1, param2);
    }

    public void SetProperty(UserPopertyKey key, string value)
    {
        if (!ready) return;
        FirebaseAnalytics.SetUserProperty(key.ToString(), value);
    }
}
