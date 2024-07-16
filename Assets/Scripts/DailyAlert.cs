using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DailyAlert : MonoBehaviour
{
    public GameObject alertIcon;
    private void Update()
    {
        var dailyRewardInfo = GameSystem.userdata.dailyRewardInfo;
        dailyRewardInfo.freeTimeRemain = FirebaseManager.remoteConfig.FREE_REWARD_INTERVAL - (DateTime.Now - dailyRewardInfo.lastFreeClaimTime).TotalSeconds;
        alertIcon.SetActive(GameSystem.userdata.dailyRewardInfo.freeTimeRemain <= 0);
    }
}
