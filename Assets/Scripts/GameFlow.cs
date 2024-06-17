using System;
using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using TMPro;
using UnityEngine;
using DG.Tweening;
using DarkcupGames;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using DeepTrackSDK;
using Vector3 = UnityEngine.Vector3;
using Vector2 = UnityEngine.Vector2;
public enum GameState
{
    Playing, Fx, Smash, Swap, Paint, Pause, GameOver
}

public class GameFlow : MonoBehaviour
{
    public static GameFlow Instance { get; private set; }
    [SerializeField] private TextMeshProUGUI scoreTxt;
    [SerializeField] private TextMeshProUGUI highScoreTxt;
    [SerializeField] private ConectedPointDisplay pointDisplay;
    [SerializeField] private Combo combo;
    [SerializeField] private BonusDiamond bonusDiamond;
    [SerializeField] private GiftButton giftButton;
    [SerializeField] private Image unmask;
    [SerializeField] private Image blockInteract;
    public Button diamondAdButton;
    public GameObject shop;
    public DiamondGroup diamondGroup;
    public Camera mainCam;
    public ButtonGroup bottomGroup;
    public ButtonGroup topGroup;
    public GameState gameState;
    [SerializeField] private PowerReward powerReward;
    private const int INIT_MULTILIER = 30;
    public List<int> multiliers = new List<int>();
    private BigInteger gameScore;
    private BigInteger totalPoint;
    private float lastGifTime;
    public float timeCount { get; private set; }

    public BigInteger GameScore
    {
        get { return gameScore; }
        set { 
            gameScore = value;
            scoreTxt.text = BigIntegerConverter.ConverNameValue(value);
            var userData = GameSystem.userdata;
            userData.gameData.currentScore = gameScore;
            if (gameScore > userData.highestScore)
            {
                userData.highestScore = gameScore;
                highScoreTxt.text = BigIntegerConverter.ConverNameValue(gameScore);
            }
            GameSystem.SaveUserDataToLocal ();
        }
    }

    public BigInteger TotalPoint
    {
        get { return totalPoint; }
        set { 
            totalPoint = value;
            if (value != 0)
            {
                pointDisplay.Show (value);
                scoreTxt.gameObject.SetActive (false);
            } else
            {
                pointDisplay.gameObject.SetActive (false);
                scoreTxt.gameObject.SetActive (true);
            }

        }
    }
    private void Awake ()
    {
        Instance = this;
        mainCam = Camera.main;
        InitMultilier ();
        pointDisplay.gameObject.SetActive (false);
        bonusDiamond.gameObject.SetActive (false);
        giftButton.gameObject.SetActive (false);
    }

    private IEnumerator Start ()
    {
        timeCount = 0;
        LoadUserData();
        LogEventButton();
        unmask.transform.localScale = Vector3.zero;
        yield return new WaitForEndOfFrame();
        if (GameSystem.userdata.replay)
        {
            PopupManager.Instance.ShowPopup(PopupOptions.StartFrom);
            unmask.transform.localScale = Vector3.one;
            blockInteract.gameObject.SetActive(false);
        } else
        {
            GridManager.Instance.effect.Prepare();
            ShowScene(GridManager.Instance.effect.RunEffect);
        }
    }

    private void Update()
    {
        timeCount += Time.deltaTime;
        if (!MaxMediationManager.rewarded.IsAdAvailable()) return;
        if(Time.time - lastGifTime > FirebaseManager.remoteConfig.GIFT_INTERVAL && GameSystem.userdata.gameData.currentHighestCellValue >= GridManager.MIN_HIGHLIGHT_VALUE)
        {
            giftButton.gameObject.SetActive (true);
            lastGifTime = Time.time;
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
        unmask.rectTransform.DOScale(UnityEngine.Vector2.one, Const.DEFAULT_TWEEN_TIME).OnComplete(() =>
        {
            AudioSystem.Instance.PlaySound("sfx_splash");
            foreach (var item in fx)
            {
                item.RunEffect();
            }
            onDone?.Invoke();
            LeanTween.delayedCall(OnSceneChangeEffect.EFFECT_TIME, () =>
            {
                blockInteract.gameObject.SetActive(false);
            });
        });
    }

    private void LoadUserData ()
    {
        var userData = GameSystem.userdata;
        GameScore = userData.gameData.currentScore;
        highScoreTxt.text = BigIntegerConverter.ConverNameValue(userData.highestScore);
    }


    private void InitMultilier ()
    {
        for (int i = 0; i <= INIT_MULTILIER; i++)
        {
            var pow = Mathf.Pow (2, i);
            multiliers.Add ((int)pow);
        }
    }

    public void SetButtonInteractacble(bool interactable)
    {
        bottomGroup.SetInteract(interactable);
        topGroup.SetInteract(interactable);
    }

    public void ShowLosePopup()
    {
        FirebaseManager.Instance.LogLevelFail(GridManager.Instance.MaxIndex, timeCount);
        DeepTrack.LogLevelLose(GridManager.Instance.MaxIndex);
        gameState = GameState.GameOver;
        PopupManager.Instance.ShowPopup(PopupOptions.Lose);
        var userData = GameSystem.userdata;
        userData.gameData.cellDic.Clear();
        GameSystem.SaveUserDataToLocal();
    }


    public void PauseGame()
    {
        if (PopupManager.Instance.GetPopup(PopupOptions.Pause).gameObject.activeInHierarchy)
            return;
        gameState = GameState.Pause;
        PopupManager.Instance.ShowPopup (PopupOptions.Pause);
    }

    public void ContinueGame () => gameState = GameState.Playing;

    public void Replay()
    {
        var userData = GameSystem.userdata;
        userData.replay = true;
        userData.gameData = new GameData();
        userData.level++;
        GameSystem.SaveUserDataToLocal ();
        unmask.transform.DOScale(Vector2.zero, Const.DEFAULT_TWEEN_TIME).OnComplete(() => SceneManager.LoadScene (SceneManager.GetActiveScene().name));
        DeepTrack.LogLevelStart(userData.level);
    }

    public void ToHome()
    {
        unmask.transform.DOScale(Vector2.zero, Const.DEFAULT_TWEEN_TIME).OnComplete(() => SceneManager.LoadScene ("UI Home"));
    }

    public void DelayToHome (float delay)
    {
        LeanTween.delayedCall (delay, ToHome);
    }

    public void CheckCombo (int count, UnityEngine.Vector3 position)
    {
        if (count < 5) return;
        combo.gameObject.SetActive (true);
        combo.ShowCombo (count, position, out var effectTime);
        var diamond = count / Const.COMBO_PER_DIAMON;
        if (diamond == 0) return;
        LeanTween.delayedCall (effectTime, () =>
        {
            bonusDiamond.transform.position = position;
            bonusDiamond.DisplayBonus (diamond, position);
            bonusDiamond.transform.DOMove (diamondGroup.transform.position, Const.DEFAULT_TWEEN_TIME).OnComplete (() => 
            {
                AudioSystem.Instance.PlaySound ("Coins_collect");
                bonusDiamond.gameObject.SetActive (false);
                diamondGroup.AddDiamond (diamond, true);
                diamondGroup.Display();
            });
        });

    }

    public void AddScore(BigInteger score)
    {
        //LeanTween.value((float)GameScore, (float)(GameScore + score), Const.DEFAULT_TWEEN_TIME).setOnUpdate((x) =>
        //{
        //    GameScore = (BigInteger)x;
        //});
        DOVirtual.Float((float)GameScore, (float)(GameScore + score), Const.DEFAULT_TWEEN_TIME, x =>
        {
            GameScore = (BigInteger)x;
        });
    }

    public void CalculateTotal (BigInteger initValue, int cellCount)
    {
        TotalPoint = initValue * (BigInteger)Mathf.Pow(2, IndexCellCount(cellCount) + 1);
        //Debug.Log(TotalPoint);
    }
    private int IndexCellCount (int cellCount)
    {
        if (cellCount == 0) return -1;
        for(var index=0;index<=INIT_MULTILIER;index++)
        {
            if (cellCount == multiliers[index]) return index;
            if (multiliers[index + 1] > cellCount && multiliers[index] <= cellCount)
                return index;
        }
        return multiliers.Count - 1;
    }

    public void GetDiamond()
    {
        diamondGroup.AddDiamond(20);
        UIManager.Instance.SpawnEffectReward(diamondAdButton.transform);
    }
    public void GetDiamond(int count)
    {
        diamondGroup.AddDiamond(count);
        UIManager.Instance.SpawnEffectReward(diamondAdButton.transform);
    }

    public void GetDiamond(Transform spawner)
    {
        if(powerReward.IsShowing()) diamondGroup.AddDiamond(80);
        else diamondGroup.AddDiamond(20);
        UIManager.Instance.SpawnEffectReward(spawner);
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
