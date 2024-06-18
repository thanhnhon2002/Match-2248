using DarkcupGames;
using DeepTrackSDK;
using Firebase.Analytics;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RewardEventLog : MonoBehaviour
{
    [SerializeField] private string placement;
    [SerializeField] private bool autoLog = true;
    private void Awake()
    {
        if (!autoLog) return;
        var button = GetComponentInChildren<Button>();
        button.onClick.AddListener(LogEvent);
 
    }

    public void LogEvent()
    {
        FirebaseManager.Instance.LogReward(placement);
        GameSystem.userdata.property.last_placement = placement;
        FirebaseManager.Instance.SetProperty(UserPopertyKey.last_placement, placement);
        GameSystem.SaveUserDataToLocal();
        MaxMediationReward.placement = placement;
        DeepTrack.Log(placement);
    }
}
