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
    private void Awake()
    {
        var button = GetComponentInChildren<Button>();
        button.onClick.AddListener(() =>
        {
            FirebaseManager.Instance.LogReward(placement);
            GameSystem.userdata.property.last_placement = placement;
            FirebaseManager.Instance.SetProperty(UserPopertyKey.last_placement, placement);
            GameSystem.SaveUserDataToLocal();
            MaxMediationReward.placement = placement;
            DeepTrack.Log(placement);
        });
    }
}
