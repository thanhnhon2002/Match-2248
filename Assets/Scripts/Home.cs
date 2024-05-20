using DarkcupGames;
using Firebase.Analytics;
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
    public DiamondGroup diamondGroup;
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
        var userData = GameSystem.userdata;
        if (!userData.firstPlayGame)
        {
            Tutorial.instance.StartTutorial();
        }
        else Invoke(nameof(MoveToGamePlay), 0.25f);
    }
    public void MoveToGamePlay()
    {
        SceneManager.LoadScene("GameplayUI");
    }
    public void GetDiamond()
    {
        diamondGroup.AddDiamond(20,false);
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
