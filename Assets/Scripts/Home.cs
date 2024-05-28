using DarkcupGames;
using Firebase.Analytics;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Home : MonoBehaviour
{
    private const int OFFER_INTERVAL_HOUR = 1;
    public static Home Instance { get; private set; }
    [SerializeField] private TextMeshProUGUI scoreTxt;
    [SerializeField] private SpecialOffer specialOffer;
    public DailyReward dailyReward;
    public Button diamondAdButton;
    public DiamondGroup diamondGroup;
    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        LogEventButton();
        var userData = GameSystem.userdata;
        scoreTxt.text = userData.highestScore.ToString();
        if (Time.time < FirebaseManager.remoteConfig.MIN_SESSION_TIME_SHOW_ADS) return;
        if (DateTime.Now.Ticks - userData.lastSpecialOffer >= TimeSpan.TicksPerHour * OFFER_INTERVAL_HOUR)
        {
            specialOffer.popup.Appear();
            userData.lastSpecialOffer = DateTime.Now.Ticks;
        } else
        {
            specialOffer.gameObject.SetActive(false);
        }
        GameSystem.SaveUserDataToLocal();
    }

    [ContextMenu("Test")]
    public void Test()
    {
        specialOffer.popup.Appear();
    }

    public void ToGameplay()
    {
        var userData = GameSystem.userdata;
        if (!userData.firstPlayGame)
        {
            Tutorial.instance.StartTutorial();
        } else Invoke(nameof(MoveToGamePlay), 0.25f);
    }
    public void MoveToGamePlay()
    {
        SceneManager.LoadScene("GameplayUI");
    }
    public void GetDiamond()
    {
        diamondGroup.AddDiamond(20, false);
        UIManager.Instance.SpawnEffectReward(diamondAdButton.transform);
    }

    private void LogEventButton()
    {
        var button = FindObjectsOfType<Button>(true);
        foreach (var item in button)
        {
            item.onClick.AddListener(() =>
            {
                FirebaseManager.Instance.LogButtonClick(item.name);
            });
        }
    }
}
