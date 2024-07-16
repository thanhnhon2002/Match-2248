using DG.Tweening;
using UnityEngine;
using System;

public class DailyReward : MonoBehaviour
{
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
        var claimList = userData.dailyRewardInfo.hasClaim;
        for (int i = 0; i < claimList.Length; i++)
        {
            claimList[i] = false;
        }
        GameSystem.SaveUserDataToLocal();
    }

    private void UpdateClaimButtonInteractable()
    {
        var dailyRewardInfo = userData.dailyRewardInfo;
        var interactableIndex = 1;
        for (int i = 1; i < dailyRewardInfo.hasClaim.Length; i++)
        {
            if (dailyRewardInfo.hasClaim[i]) interactableIndex = i;
        }
        interactableIndex++;
        for (int i = 0; i < rewards.Length; i++)
        {
            if (rewards[i].index == 0)
            {
                rewards[i].CheckFreeGift();
                continue;
            }
            rewards[i].SetClaimButtonInteractable(interactableIndex);
        }
    }
}
