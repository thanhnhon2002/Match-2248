using DarkcupGames;
using DG.Tweening;
using System.Collections.Generic;
using UnityEngine;

public class Hammer : Power
{
    public static Hammer Instance { get; private set; }
    [SerializeField] private Animator hammer;
    [SerializeField] private RectTransform hamerImg;
    [SerializeField] private ParticalSystemController smashFx;
    [SerializeField] private AudioClip cellSmashSound;
    private bool chose;
    private Cell cell;
    private void Awake ()
    {
        Instance = this;
    }
    public override void UsePower ()
    {
        if (GameFlow.Instance.gameState != GameState.Playing) return;
        if (GameSystem.userdata.diamond < cost)
        {
            GameFlow.Instance.shop.SetActive(true);
            return;
        }
        cell = null;
        base.UsePower ();
        GameFlow.Instance.gameState = GameState.Smash;
        hammer.SetBool ("Play", true);
        chose = false;
    }

    public void Smash(Cell cell)
    {
        if (this.cell != null) return;
        this.cell = cell;
        GameSystem.userdata.diamond -= cost;
        GameFlow.Instance.diamondGroup.Display();
        if (chose) return;
        chose = true;
        backButton.gameObject.SetActive (false); 
        hammer.SetBool ("Play", false);
        var destination = GameFlow.Instance.mainCam.WorldToScreenPoint (cell.transform.position);
        var position = hamerImg.position;
        var sq = DOTween.Sequence ();
        sq.AppendCallback (() => 
        {
            hamerImg.DOMove (destination, 1f).OnComplete (() => hammer.Play ("HammerOne"));
        });
        sq.AppendInterval (1.5f);
        sq.AppendCallback (() => 
        {
            RemoveCell (cell);
            displayGroup.DOFade (0f, 1f).OnComplete(() => 
            {
                hamerImg.anchoredPosition = Vector3.zero;
                GameFlow.Instance.bottomGroup.gameObject.SetActive (true);
                GameFlow.Instance.topGroup.gameObject.SetActive (true);
                displayGroup.gameObject.SetActive (false);
                displayGroup.alpha = 1f;
            });
        });

    }


    private void RemoveCell (Cell cell)
    {
        var collomIndex = cell.gridPosition.x;
        var cellsInSameCol = GridManager.Instance.allCellInCollom[collomIndex];
        var fx = PoolSystem.Instance.GetObject(smashFx, cell.transform.position); 
        fx.ChangeColor(cell.spriteRenderer.color);
        cell.gameObject.SetActive (false);
        AudioSystem.Instance.PlaySound (cellSmashSound);
        var spawnPos = GridManager.Instance.GetCellAt (new GridPosition (collomIndex, 1)).transform.position;
        spawnPos.y++;

        cellsInSameCol.Remove (cell);
        var newCell = GridManager.Instance.SpawnCellNew (spawnPos);
        cellsInSameCol.Add (newCell);

        cellsInSameCol.Sort ((a, b) => a.transform.localPosition.y.CompareTo (b.transform.localPosition.y));

        GridManager.Instance.ReassignGridPos (collomIndex, cellsInSameCol);
        LeanTween.delayedCall (0.5f, () => GridManager.Instance.Drop ());
    }
}
