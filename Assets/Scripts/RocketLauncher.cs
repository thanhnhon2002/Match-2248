using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using DG.Tweening;
using System.Net.Sockets;
using DarkcupGames;

public class RocketLauncher : Power<RocketLauncher>
{
    private const int TARGET_AMOUNT = 5;
    private const float ROCKET_MOVE_TIME = 1f;
    private List<Cell> targets = new List<Cell>();
    private List<Cell> allCells = new List<Cell>();
    [SerializeField] private Rocket rocketPre;
    [SerializeField] private ParticalSystemController smashFx;
    [SerializeField] private AudioClip explosedSound;

    public override void UsePower()
    {
        if (GameFlow.Instance.gameState != GameState.Playing) return;
        if (GameSystem.userdata.diamond < cost)
        {
            GameFlow.Instance.shop.SetActive(true);
            return;
        }
        base.UsePower();
        LaunchRocket();
    }

    public void LaunchRocket()
    {
        info.isTutorialFinish = true;
        GameSystem.SaveUserDataToLocal();
        GameFlow.Instance.gameState = GameState.Fx; 
        targets.Clear();
        allCells.Clear();
        allCells.AddRange (GridManager.Instance.allCell);
        allCells.Sort ((a, b) => a.Value.CompareTo(b.Value));
        for (int i = 0; i < TARGET_AMOUNT; i++)
        {
            targets.Add (allCells[i]);
        }
        targets.Add (null);
        var sq = DOTween.Sequence ();
        sq.AppendCallback (() => 
        {
            for (int i = 0; i < TARGET_AMOUNT; i++)
            {
                var rocket = PoolSystem.Instance.GetObject (rocketPre, transform.position);
                rocket.target = allCells[i].transform.position;
                rocket.transform.DOMove (allCells[i].transform.position, ROCKET_MOVE_TIME).SetEase (Ease.Linear).OnComplete (() => rocket.gameObject.SetActive (false)); ;
            }
        });
        sq.AppendInterval (ROCKET_MOVE_TIME * 1.5f);
        sq.AppendCallback (() => 
        {
            AudioSystem.Instance.PlaySound (explosedSound);
            for (int i = 0; i < TARGET_AMOUNT; i++)
            {
                allCells[i].gameObject.SetActive (false);
                var fx = PoolSystem.Instance.GetObject (smashFx, allCells[i].transform.position);
                fx.ChangeColor (allCells[i].spriteRenderer.color);
            }
        });
        sq.AppendInterval (1f);
        sq.AppendCallback (() =>
        {
            GridManager.Instance.CheckToSpawnNewCell(targets);
            Back();
            onUseCompleted?.Invoke();
        });
    }

    public void PayToLaunchRocket()
    {
        var userData = GameSystem.userdata;
        if (userData.diamond < cost)
        {
            GameFlow.Instance.shop.SetActive(true);
            return;
        }
        if (!ignoreCost) userData.diamond -= cost;
        PopupManager.Instance.HidePopup(PopupOptions.Lose);
        info.isTutorialFinish = true;
        GameSystem.SaveUserDataToLocal();
        LaunchRocket();
        GameFlow.Instance.diamondGroup.Display();
    }

    public override void UsePowerIgnoreCost()
    {
        base.UsePowerIgnoreCost ();
        base.UsePower();
        LaunchRocket();
    }
}
