using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Firebase.Extensions;
using Firebase;

public enum AnalyticsEvent
{
    level_start, level_passed, level_failed, will_show_interstitial, will_show_rewarded, ui_appear, button_click
}

public class FirebaseManager : MonoBehaviour
{
    public static FirebaseManager Instance;
    public FirebaseApp app;
    public bool ready;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        } else Destroy(gameObject);
    }

    IEnumerator Start()
    {
        yield return FirebaseApp.CheckAndFixDependenciesAsync().ContinueWithOnMainThread(task =>
        {
            var dependencyStatus = task.Result;
            if (dependencyStatus == DependencyStatus.Available)
            {
                app = FirebaseApp.DefaultInstance;
                ready = true;
                Debug.Log("Firebas is ready");
            } else
            {
                Debug.LogError(System.String.Format(
                  "Could not resolve all Firebase dependencies: {0}", dependencyStatus));
            }
        });
    }

    public void LogEvent(AnalyticsEvent type, string parameter)
    {
        Firebase.Analytics.FirebaseAnalytics.LogEvent(
            Firebase.Analytics.FirebaseAnalytics.EventJoinGroup,
            Firebase.Analytics.FirebaseAnalytics.ParameterGroupId,
            $"{type} {parameter}");
    }
}
