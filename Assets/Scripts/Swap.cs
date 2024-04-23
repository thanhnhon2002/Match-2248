using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Swap : Power
{
    public static Swap Instance { get; private set; } 
    private List<Cell> chosenCell = new List<Cell>();
    private List<CellHighlight> cellHighlight = new List<CellHighlight>();
    [SerializeField] private CellHighlight highlightPre;

    private void Awake ()
    {
        Instance = this;
    }

    public override void UsePower ()
    {
        base.UsePower ();
        GameFlow.Instance.gameState = GameState.Swap;
    }

    public void ChoseCell(Cell cell)
    {
        if (chosenCell.Contains (cell))
        {
            var highlight = cellHighlight.Find(x => x.cell.Equals (cell));
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
        if(chosenCell.Count >= 2)
        {
            GameFlow.Instance.gameState = GameState.Fx;
            var pos0 = chosenCell[0].transform.localPosition;
            var gridPos0 = chosenCell[0].gridPosition;
            var pos1 = chosenCell[1].transform.localPosition;
            var gridPos1 = chosenCell[1].gridPosition;

            chosenCell[0].gridPosition = gridPos1;
            chosenCell[1].gridPosition = gridPos0;

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
                chosenCell.Clear ();
                cellHighlight.Clear ();
            });
        }
    }
}
