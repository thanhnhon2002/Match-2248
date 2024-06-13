using DarkcupGames;
using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Swap : Power<Swap>
{
    private List<Cell> chosenCell = new List<Cell> ();
    private List<CellHighlight> cellHighlight = new List<CellHighlight> ();
    [SerializeField] private CellHighlight highlightPre;
    [SerializeField] private AudioClip interactSound;

    public override void UsePower ()
    {
        if (GameFlow.Instance.gameState != GameState.Playing) return;
        if (GameSystem.userdata.diamond < cost)
        {
            GameFlow.Instance.shop.SetActive(true);
            return;
        }
        base.UsePower ();
        GameFlow.Instance.gameState = GameState.Swap;
    }

    public void ChoseCell (Cell cell)
    {
        AudioSystem.Instance.PlaySound(interactSound);
        if (chosenCell.Contains (cell))
        {
            var highlight = cellHighlight.Find (x => x.cell.Equals (cell));
            highlight.gameObject.SetActive (false);
            chosenCell.Remove (cell);
            cellHighlight.Remove (highlight);
        } else
        {
            chosenCell.Add (cell);
            var highligt = PoolSystem.Instance.GetObject (highlightPre, cell.transform.position);
            highligt.cell = cell;
            cellHighlight.Add (highligt);
        }
        if (chosenCell.Count < 2) return;
        GameSystem.userdata.diamond -= cost;
        GameFlow.Instance.diamondGroup.Display();
        backButton.gameObject.SetActive (false);
        for (int i = 0; i < chosenCell.Count; i++)
        {
            var list = GridManager.Instance.allCellInCollom[chosenCell[i].gridPosition.x];
            list.Remove (chosenCell[i]);

        }
        GameFlow.Instance.gameState = GameState.Fx;
        var firstCell = chosenCell[0];
        var otherCell = chosenCell[1];

        var firstList = GridManager.Instance.allCellInCollom[firstCell.gridPosition.x];
        var otherList = GridManager.Instance.allCellInCollom[otherCell.gridPosition.x];

        firstList.Add (otherCell);
        otherList.Add (firstCell);
        var pos0 = firstCell.transform.localPosition;
        var gridPos0 = firstCell.gridPosition;
        var pos1 = otherCell.transform.localPosition;
        var gridPos1 = otherCell.gridPosition;

        firstCell.gridPosition = gridPos1;
        otherCell.gridPosition = gridPos0;

        var sq = DOTween.Sequence ();
        sq.AppendCallback (() =>
        {
            chosenCell[0].transform.DOLocalMove (pos1, Const.DEFAULT_TWEEN_TIME);
            chosenCell[1].transform.DOLocalMove (pos0, Const.DEFAULT_TWEEN_TIME);
            var rotation = Vector3.zero;
            LeanTween.value (360f, 0f, Const.DEFAULT_TWEEN_TIME).setOnUpdate (x =>
            {
                for (int i = 0; i < chosenCell.Count; i++)
                {
                    rotation.z = x;
                    chosenCell[i].transform.eulerAngles = rotation;
                }
            });
        });
        sq.AppendInterval (0.5f);
        sq.AppendCallback (() =>
        {
            for (int i = 0; i < chosenCell.Count; i++)
            {
                chosenCell[i].transform.eulerAngles = Vector3.zero;
            }
            for (int i = 0; i < cellHighlight.Count; i++)
            {
                cellHighlight[i].gameObject.SetActive (false);
            }
            GridManager.Instance.OnDoneCellMove ();
            displayGroup.gameObject.SetActive (false);
            GameFlow.Instance.bottomGroup.gameObject.SetActive (true);
            GameFlow.Instance.topGroup.gameObject.SetActive (true);
            chosenCell.Clear ();
            cellHighlight.Clear ();
        });
    }

    public override void Back ()
    {
        base.Back ();
        chosenCell.Clear ();
        for (int i = 0; i < cellHighlight.Count; i++)
        {
            cellHighlight[i].gameObject.SetActive (false);
        }
        cellHighlight.Clear ();
    }
}
