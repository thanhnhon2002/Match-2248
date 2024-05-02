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
        if(index > 0) FirebaseManager.Instance.LogEvent(AnalyticsEvent.will_show_rewarded,
            $"internet_available {Application.internetReachability}, placement daily reward {index}, has_ads {AdManagerMax.Instance.isCurrentAdAvaiable}");
        Home.Instance.dailyReward.gameObject.SetActive(false);
        var userData = GameSystem.userdata;
        userData.dailyRewardInfo.hasClaim[index] = true;
        userData.diamond += amount;
        GameSystem.SaveUserDataToLocal();
        UIManager.Instance.SpawnEffectReward(claimButton.transform.position);
    }
}
