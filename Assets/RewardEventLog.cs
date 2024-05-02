using DarkcupGames;
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
        var button = GetComponent<Button>();
        button.onClick.AddListener(() =>
        {
            FirebaseManager.Instance.LogReward(placement);
        });
    }
}
