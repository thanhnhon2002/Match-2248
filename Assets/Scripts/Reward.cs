using DarkcupGames;
using DeepTrackSDK;
using DG.Tweening.Core.Easing;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting.Antlr3.Runtime.Tree;
using UnityEngine;
using UnityEngine.UI;

public class Reward : MonoBehaviour
{
    private const float FREE_INTERVAL = 600;
    [SerializeField] private int amount;
    [SerializeField] private TimeCounter timeCounter;
    [SerializeField] private TextMeshProUGUI free;
    public string placement;
    public Button claimButton;
    public int index;

    public void GetReward()
    {
        var userData = GameSystem.userdata;
        userData.dailyRewardInfo.HasClaim[index] = true;
        if(index == 0)
        {
            userData.dailyRewardInfo.lastFreeClaimTime = DateTime.Now;
            userData.dailyRewardInfo.freeTimeRemain = FREE_INTERVAL;
        }
        DiamondGroup.Instance.AddDiamond(amount,false);
        UIManager.Instance.SpawnEffectReward(claimButton.transform);
        Home.Instance.dailyReward.gameObject.SetActive(false);
        GameSystem.SaveUserDataToLocal();
        FirebaseManager.Instance.LogReward(placement);
        DeepTrack.Log(placement);
    }

    public void CheckFreeGift()
    {
        if (index != 0) return;
        var dailyRewardInfo = GameSystem.userdata.dailyRewardInfo;
        dailyRewardInfo.freeTimeRemain -= (DateTime.Now - dailyRewardInfo.lastFreeClaimTime).TotalSeconds;
        if (!dailyRewardInfo.HasClaim[0])
        {
            claimButton.interactable = true;
        } else if (dailyRewardInfo.freeTimeRemain <= 0)
        {
            claimButton.interactable = true;
        } else
        {
            claimButton.interactable = false;
        }

        if (claimButton.interactable)
        {
            timeCounter.gameObject.SetActive(false);
            free.gameObject.SetActive(true);
        } else
        {
            timeCounter.gameObject.SetActive(true);
            free.gameObject.SetActive(false);
            timeCounter.SetTime(dailyRewardInfo.freeTimeRemain);
        }
        GameSystem.SaveUserDataToLocal();
    }
}
