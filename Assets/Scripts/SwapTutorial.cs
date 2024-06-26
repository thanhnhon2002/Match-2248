using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using UnityEngine;
using UnityEngine.UI;

public class SwapTutorial : PowerTutorial
{
    [SerializeField] private Image[] unmasks;

    public override void DoTutorial()
    {
        foreach (var item in unmasks) 
        {
            item.rectTransform.localScale = UnityEngine.Vector3.one * 30f;
        }
        var allCells = GridManager.Instance.GetComponentsInChildren<Cell>().ToList();
        allCells.Sort((a, b) => b.Value.CompareTo(a.Value));

        var matchingIndex = 0;
        var cell = allCells[matchingIndex];

        foreach (var item in allCells)
        {
            if (allCells.Count(x => x.Value == cell.Value) > 1) break;
            matchingIndex++;
            cell = allCells[matchingIndex];
        }

        var target1 = cell.nearbyCell.First();
        var pos = GameFlow.Instance.mainCam.WorldToScreenPoint(target1.transform.position);
        allCells.Remove(target1);
        foreach (var item in target1.nearbyCell)
        {
            allCells.Remove(item);
        }
        unmasks[0].transform.position = pos;
        unmasks[0].transform.DOScale(1f, 1f);

        var target2 = allCells.First(x => x.Value != cell.Value);
        pos = GameFlow.Instance.mainCam.WorldToScreenPoint(target2.transform.position);
        unmasks[1].transform.position = pos;
        unmasks[1].transform.DOScale(1f, 1f);
    }
}
