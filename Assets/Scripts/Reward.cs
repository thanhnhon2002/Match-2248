using DarkcupGames;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Reward : MonoBehaviour
{
    [SerializeField] private int amount;
    public Button claimButton;
    public int index;

    public void GetReward()
    {
        var userData = GameSystem.userdata;
        userData.dailyRewardInfo.hasClaim[index] = true;
        PlayerPrefs.SetInt(DailyReward.HAS_CLAIM + index, 1);
        DiamondGroup.Instance.AddDiamond(amount,false);
        UIManager.Instance.SpawnEffectReward(claimButton.transform);
        Home.Instance.dailyReward.gameObject.SetActive(false);
    }
}
