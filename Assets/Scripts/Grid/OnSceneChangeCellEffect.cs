using System.Collections;
using UnityEngine;
using DG.Tweening;

public class OnSceneChangeCellEffect : MonoBehaviour, ISceneChangeEffect
{
    private const float EFFEC_DELAY = 0.01F;

    public void Prepare()
    {
        foreach (var item in GridManager.Instance.allCell)
        {
            item.transform.localPosition = new Vector3(item.transform.localPosition.x, 7);
            item.transform.localScale = Vector3.zero;
            item.valueTxt.alpha = 0f;
            item.gameObject.SetActive(true);
        }
    }

    public void RunEffect()
    {
        StartCoroutine(CellToPos());
    }

    private IEnumerator CellToPos()
    {
        //var allCells = GridManager.Instance.allCell;
        //for (int i = allCells.Length - 1; i >= 0; i--)
        //{
        //    allCells[i].transform.DOScale(0.8f, Const.DEFAULT_TWEEN_TIME);
        //    allCells[i].transform.DOLocalMove(GridManager.Instance.girdPosToLocal[allCells[i].gridPosition], Const.DEFAULT_TWEEN_TIME);
        //    allCells[i].valueTxt.DOFade(1f, Const.DEFAULT_TWEEN_TIME);
        //    yield return new WaitForSeconds(EFFEC_DELAY);
        //}
        for (int i = GridManager.Instance.MAX_ROW; i >= 1; i--)
        {
            for (int j = GridManager.Instance.MAX_COL; j >= 1; j--)
            {
                var grid = new GridPosition(j, i);
                var cell = GridManager.Instance.GetCellAt(grid);
                cell.transform.DOScale(0.8f, Const.DEFAULT_TWEEN_TIME);
                cell.transform.DOLocalMove(GridManager.Instance.girdPosToLocal[cell.gridPosition], Const.DEFAULT_TWEEN_TIME);
                cell.valueTxt.DOFade(1f, Const.DEFAULT_TWEEN_TIME);
                yield return new WaitForSeconds(EFFEC_DELAY);
            }
        }
    }
}
