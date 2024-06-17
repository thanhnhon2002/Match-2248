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
        UpdateClaimButtonInteractable();
    }
    public void Close()
    {
        transform.DOScale(0f, Const.DEFAULT_TWEEN_TIME).OnComplete(() => gameObject.SetActive(false));
    }

    public void ResetReward()
    {
        var claimList = userData.dailyRewardInfo.HasClaim;
        for (int i = 0; i < claimList.Count; i++)
        {
            claimList[i] = false;
        }
        GameSystem.SaveUserDataToLocal();
    }

    private void UpdateClaimButtonInteractable()
    {
        var dailyRewardInfo = userData.dailyRewardInfo;
        for (int i = 0; i < rewards.Length; i++)
        {
            if (rewards[i].index == 0)
            {
                rewards[i].CheckFreeGift();
                continue;
            }
            rewards[i].claimButton.interactable = rewards[i].index >= 1 && dailyRewardInfo.HasClaim[rewards[i].index - 1] && !dailyRewardInfo.HasClaim[rewards[i].index];
        }
    }
}
