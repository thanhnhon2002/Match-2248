using DarkcupGames;
using DeepTrackSDK;
using DG.Tweening.Core.Easing;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting.Antlr3.Runtime.Tree;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Reward : MonoBehaviour
{
    [SerializeField] private int amount;
    [SerializeField] private TimeCounter timeCounter;
    [SerializeField] private TextMeshProUGUI free;
    [SerializeField] private GameObject unlockDisplay;
    [SerializeField] private GameObject lockDisplay;
    [SerializeField] private GameObject claimDisplay;
    public string placement;
    public Button claimButton;
    public int index;

    public void GetReward()
    {
        var userData = GameSystem.userdata;
        userData.dailyRewardInfo.hasClaim[index] = true;
        if(index == 0)
        {
            userData.dailyRewardInfo.lastFreeClaimTime = DateTime.Now;
            userData.dailyRewardInfo.freeTimeRemain = FirebaseManager.remoteConfig.FREE_REWARD_INTERVAL;
        }
        DiamondGroup.Instance.AddDiamond(amount);
        DiamondGroup.Instance.Display();
        UIManager.Instance.SpawnEffectReward(claimButton.transform);
        if(SceneManager.GetActiveScene().name == "UI Home")
        {
        Home.Instance.dailyReward.gameObject.SetActive(false);

        }
        else
        {
            FindAnyObjectByType<DailyReward>().gameObject.SetActive(false);
        }
        GameSystem.SaveUserDataToLocal();
        FirebaseManager.Instance.LogReward(placement);
        DeepTrack.Log(placement);
    }

    public void CheckFreeGift()
    {
        if (index != 0) return;
        var dailyRewardInfo = GameSystem.userdata.dailyRewardInfo;
        dailyRewardInfo.freeTimeRemain = FirebaseManager.remoteConfig.FREE_REWARD_INTERVAL - (DateTime.Now - dailyRewardInfo.lastFreeClaimTime).TotalSeconds;
        if (!dailyRewardInfo.hasClaim[0])
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

    public void SetClaimButtonInteractable(int currentInteractableIndex)
    {
        if(index > currentInteractableIndex)
        {
            claimButton.interactable = false;
            lockDisplay.SetActive(true);
            claimDisplay.SetActive(false);
            unlockDisplay.SetActive(false);
        }
        else if(index < currentInteractableIndex)
        {
            claimButton.interactable = false;
            lockDisplay.SetActive(false);
            claimDisplay.SetActive(true);
            unlockDisplay.SetActive(false);
        } else
        {
            claimButton.interactable = true;
            unlockDisplay.SetActive(true);
            lockDisplay.SetActive(false);
            claimDisplay.SetActive(false);
        }


    }
}
