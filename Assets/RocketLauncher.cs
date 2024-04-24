using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using DG.Tweening;
using System.Net.Sockets;

public class RocketLauncher : MonoBehaviour
{
    private const int TARGET_AMOUNT = 5;
    private const float ROCKET_MOVE_TIME = 1f;
    public static RocketLauncher Instance { get; private set;}
    private List<Cell> targets = new List<Cell>();
    private List<Cell> allCells = new List<Cell>();
    [SerializeField] private Rocket rocketPre;
    [SerializeField] private ParticalSystemController smashFx;
    private void Awake ()
    {
        Instance = this;
    }
    [ContextMenu("Test")]
    public void LaunchRocket()
    {
        targets.Clear();
        allCells.Clear();
        var allCell = GridManager.Instance.allCell;
        allCells.AddRange (allCell);
        allCells.Sort ((a, b) => a.Value.CompareTo (b.Value));
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
            for (int i = 0; i < TARGET_AMOUNT; i++)
            {
                allCells[i].gameObject.SetActive (false);
                var fx = PoolSystem.Instance.GetObject (smashFx, allCells[i].transform.position);
                fx.ChangeColor (allCells[i].spriteRenderer.color);
            }
        });
        sq.AppendInterval (1f);
        sq.AppendCallback (() => GridManager.Instance.CheckToSpawnNewCell (targets));
    }
}
