using System;
using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using TMPro;
using UnityEngine;
using DG.Tweening;
using DarkcupGames;
using UnityEngine.SceneManagement;

public enum GameState
{
    Playing, Fx, Smash, Swap,Pause
}

public class GameFlow : MonoBehaviour
{
    public static GameFlow Instance { get; private set; }
    [SerializeField] private TextMeshProUGUI scoreTxt;
    [SerializeField] private TextMeshProUGUI highScoreTxt;
    [SerializeField] private ConectedPointDisplay pointDisplay;
    [SerializeField] private Combo combo;
    [SerializeField] private DiamondGroup diamondGroup;
    [SerializeField] private BonusDiamond bonusDiamond;
    public Camera mainCam;
    public GameObject bottomGroup;
    public GameState gameState;
    private const int INIT_MULTILIER = 30;
    public List<int> multiliers = new List<int>();
    private BigInteger gameScore;
    private BigInteger totalPoint;

    public BigInteger GameScore
    {
        get { return gameScore; }
        set { 
            gameScore = value;
            scoreTxt.text = value.ToString ();
            var userData = GameSystem.userdata;
            userData.currentScore = gameScore;
            if (gameScore > userData.highestScore)
            {
                userData.highestScore = gameScore;
                highScoreTxt.text = gameScore.ToString ();
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
    }

    private void Start ()
    {
        LoadUserData ();
        AudioSystem.Instance.PlaySound ("Game_Open");
    }

    private void LoadUserData ()
    {
        var userData = GameSystem.userdata;
        GameScore = userData.currentScore;
        highScoreTxt.text = userData.highestScore.ToString ();
    }


    private void InitMultilier ()
    {
        for (int i = 0; i <= INIT_MULTILIER; i++)
        {
            var pow = Mathf.Pow (2, i);
            multiliers.Add ((int)pow);
        }
    }

    public void ShowLosePopup()
    {

    }

    public void PauseGame()
    {
        gameState = GameState.Pause;
        PopupManager.Instance.ShowPopup (PopupOptions.Pause);
    }

    public void ContinueGame () => gameState = GameState.Playing;

    public void Replay()
    {
        var userData = GameSystem.userdata;
        userData.cellDic.Clear ();
        GameSystem.SaveUserDataToLocal ();
        SceneManager.LoadScene (SceneManager.GetActiveScene ().name);
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
            });
        });

    }

    public void AddScore(BigInteger score)
    {
        LeanTween.value ((float)GameScore, (float)(GameScore + score), Const.DEFAULT_TWEEN_TIME).setOnUpdate ((x) => 
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
}
