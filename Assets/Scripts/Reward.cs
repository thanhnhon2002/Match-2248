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
        Home.Instance.dailyReward.gameObject.SetActive(false);
        var userData = GameSystem.userdata;
        userData.dailyRewardInfo.hasClaim[index] = true;
        userData.diamond += amount;
        GameSystem.SaveUserDataToLocal();
        UIManager.Instance.SpawnEffectReward(claimButton.transform);
    }
}
