using DG.Tweening;
using UnityEngine;
using System;

public class DailyReward : MonoBehaviour
{
    public const string LAST_REWARD_TICK = "LastRewardTick";
    public const string HAS_CLAIM = "HasClaim";
    private Reward[] rewards;
    private UserData userData;


    private void Awake()
    {
        rewards = GetComponentsInChildren<Reward>();
        userData = GameSystem.userdata;
    }

    private void OnEnable()
    {
        FirebaseManager.Instance.LogUIAppear(gameObject.name);
        transform.localScale = Vector3.one;
        CheckData();
        UpdateClaimButtonInteractable();

    }
    public void Close()
    {
        transform.DOScale(0f, Const.DEFAULT_TWEEN_TIME).OnComplete(() => gameObject.SetActive(false));
    }
    private void CheckData()
    {
        LoadDailyRewardInfo();
        if (DateTime.Now.Ticks - userData.dailyRewardInfo.lastRewardTick >= TimeSpan.TicksPerDay)
        {
            userData.dailyRewardInfo.lastRewardTick = DateTime.Now.Ticks;
            var claimList = userData.dailyRewardInfo.hasClaim;
            for (int i = 0; i < claimList.Count; i++)
            {
                claimList[i] = false;
            }
            SaveDailyRewardInfo();
        }
    }
    private void LoadDailyRewardInfo()
    {
        if (PlayerPrefs.HasKey(LAST_REWARD_TICK))
        {
            userData.dailyRewardInfo.lastRewardTick = long.Parse(PlayerPrefs.GetString(LAST_REWARD_TICK));
        }

        for (int i = 0; i < rewards.Length; i++)
        {
            if (PlayerPrefs.HasKey(HAS_CLAIM + i))
            {
                userData.dailyRewardInfo.hasClaim[i] = PlayerPrefs.GetInt(HAS_CLAIM + i) == 1;
            }
        }
    }
    private void SaveDailyRewardInfo()
    {
        PlayerPrefs.SetString(LAST_REWARD_TICK, userData.dailyRewardInfo.lastRewardTick.ToString());
        for (int i = 0; i < userData.dailyRewardInfo.hasClaim.Count; i++)
        {
            PlayerPrefs.SetInt(HAS_CLAIM + i, userData.dailyRewardInfo.hasClaim[i] ? 1 : 0);
        }
        PlayerPrefs.Save();
    }
    private void UpdateClaimButtonInteractable()
    {
        var dailyRewardInfo = userData.dailyRewardInfo;
        for (int i = 0; i < rewards.Length; i++)
        {
            rewards[i].claimButton.interactable = !dailyRewardInfo.hasClaim[rewards[i].index];
        }
    }
}
