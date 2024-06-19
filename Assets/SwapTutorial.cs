using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class SwapTutorial : PowerTutorial
{
    [SerializeField] private Image[] unmasks;

    [ContextMenu("Run")]
    public override void DoTutorial()
    {
        foreach (var item in unmasks) 
        {
            item.rectTransform.localScale = Vector3.one * 30f;
        }
        var allCells = GridManager.Instance.GetComponentsInChildren<Cell>().ToList();
        allCells.Sort((a, b) => b.Value.CompareTo(a.Value));

        var target1 = allCells[0].nearbyCell.First(x => x.Value != allCells[0].Value);
        var pos = GameFlow.Instance.mainCam.WorldToScreenPoint(target1.transform.position);
        unmasks[0].transform.position = pos;
        unmasks[0].transform.DOScale(1f, 1f);

        var target2 = allCells.Last();
        pos = GameFlow.Instance.mainCam.WorldToScreenPoint(target2.transform.position);
        unmasks[1].transform.position = pos;
        unmasks[1].transform.DOScale(1f, 1f);
    }
}
