using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System;

public class DailyReward : MonoBehaviour
{
    private Reward[] rewards;

    private void Awake()
    {
        rewards = GetComponentsInChildren<Reward>();
    }

    private void OnEnable()
    {
        FirebaseManager.Instance.LogEvent(AnalyticsEvent.ui_appear, $"screen_name {SceneManager.GetActiveScene().name}, name {gameObject.name}");
        CheckData();
        transform.localScale = Vector3.one;
        var userData = GameSystem.userdata.dailyRewardInfo;
        for (int i = 0; i < rewards.Length; i++)
        {
            rewards[i].claimButton.interactable = !userData.hasClaim[rewards[i].index];
        }
    }

    public void Close()
    {
        transform.DOScale(0f, Const.DEFAULT_TWEEN_TIME).OnComplete(() => gameObject.SetActive(false));
    }

    private void CheckData()
    {
        var userData = GameSystem.userdata;
        if( DateTime.Now.Ticks - userData.dailyRewardInfo.lastRewardTick >= TimeSpan.TicksPerDay)
        {
            userData.dailyRewardInfo.lastRewardTick = DateTime.Now.Ticks;
            var claimList = userData.dailyRewardInfo.hasClaim;
            for (int i = 0; i < claimList.Count; i++)
            {
                claimList[i] = false;
            }
        }
    }
}
