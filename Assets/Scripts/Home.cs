using DarkcupGames;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Home : MonoBehaviour
{
    public static Home Instance { get; private set; }
    [SerializeField] private TextMeshProUGUI scoreTxt;
    public DailyReward dailyReward;
    public Button diamondAdButton;
    private void Awake ()
    {
        Instance = this;
    }

    private void Start ()
    {
        LogEventButton();
        var userData = GameSystem.userdata;
        scoreTxt.text = userData.highestScore.ToString ();
    }

    public void ToGameplay()
    {
        Invoke(nameof(MoveToGamePlay), 0.25f);
    }
    public void MoveToGamePlay()
    {
        FirebaseManager.Instance.LogEvent(AnalyticsEvent.level_start, "restart false");
        SceneManager.LoadScene("GameplayUI");
    }
    public void GetDiamond()
    {
        FirebaseManager.Instance.LogEvent(AnalyticsEvent.will_show_rewarded, 
            $"internet_available {Application.internetReachability}, placement GetDiamond Home, has_ads {AdManagerMax.Instance.isCurrentAdAvaiable}");
        GameSystem.userdata.diamond += 20;
        GameSystem.SaveUserDataToLocal();
        UIManager.Instance.SpawnEffectReward(diamondAdButton.transform.position);
    }

    private void LogEventButton()
    {
        var button = FindObjectsOfType<Button>(true);
        foreach (var item in button)
        {
            item.onClick.AddListener(() => FirebaseManager.Instance.LogEvent(AnalyticsEvent.button_click, $"Home, name {item.name}"));
        }
    }
}
