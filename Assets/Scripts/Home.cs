using DarkcupGames;
using DeepTrackSDK;
using DG.Tweening;
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
    public static Home Instance { get; private set; }
    [SerializeField] private TextMeshProUGUI scoreTxt;
    [SerializeField] private SpecialOffer specialOffer;
    [SerializeField] private Image unmask;
    [SerializeField] private Image blockInteract;
    [SerializeField] FadeInOut fadeInOut;

    public DailyReward dailyReward;
    public Button diamondAdButton;
    public DiamondGroup diamondGroup;
    private void Awake()
    {
        Instance = this;
    }

    private IEnumerator Start()
    {
        if (GameSystem.userdata.firstPlayGame) Tutorial.instance.StartTutorial();
        if (fadeInOut) fadeInOut.canvasGroup.alpha = 0;
        yield return new WaitForEndOfFrame();
        if (fadeInOut) fadeInOut.FadeIn(null);
        unmask.transform.localScale = Vector3.zero;
        ShowScene(LoadUserData);
        LogEventButton();
    }

    private void LoadUserData()
    {
        var userData = GameSystem.userdata;
        scoreTxt.text = BigIntegerConverter.ConvertNameValue(userData.highestScore);
        if (userData.firstPlayGame) return;
        if (userData.boughtItems.Contains(IAP_ID.no_ads.ToString())) return;
        if (Time.time < FirebaseManager.remoteConfig.MIN_SESSION_TIME_SHOW_ADS) return;
        if (DateTime.Now.Ticks - userData.lastSpecialOffer >= TimeSpan.TicksPerMinute * FirebaseManager.remoteConfig.MIN_MINUTE_SPECIAL_OFFER)
        {
            specialOffer.popup.Appear();
            userData.lastSpecialOffer = DateTime.Now.Ticks;
        } else
        {
            specialOffer.gameObject.SetActive(false);
        }
        GameSystem.SaveUserDataToLocal();
    }

    public void ToGameplay()
    {
        Invoke(nameof(MoveToGamePlay), 0.25f);
    }
    public void MoveToGamePlay()
    {
        unmask.transform.DOScale(0f, Const.DEFAULT_TWEEN_TIME).OnComplete(() => SceneManager.LoadScene("GameplayUI"));
        GameSystem.userdata.level++;
        GameSystem.SaveUserDataToLocal();
        DeepTrack.LogLevelStart(GameSystem.userdata.level);
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

    private void ShowScene(Action onDone = null)
    {
        blockInteract.gameObject.SetActive(true);
        var fx = FindObjectsOfType<OnSceneChangeEffect>();
        foreach (var item in fx)
        {
            item.Prepare();
        }
        unmask.rectTransform.DOScale(Vector2.one, Const.DEFAULT_TWEEN_TIME).OnComplete(() =>
        {
            foreach (var item in fx)
            {            
                item.RunEffect();
            }
            LeanTween.delayedCall(Const.DEFAULT_TWEEN_TIME + 0.3f, () =>
            {
                blockInteract.gameObject.SetActive(false);
                onDone?.Invoke();
            });
        });
    }
}
