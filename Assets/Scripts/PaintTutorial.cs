using DG.Tweening;
using NSubstitute.Core;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.Collections;
using UnityEngine;

public class PaintTutorial : PowerTutorial
{
    [SerializeField] private SpriteRenderer hand;
    [SerializeField] private Sprite pressSprite;
    [SerializeField] private Sprite releaseSprite;
    [SerializeField] private Line linePrefab;

    private LinkedList<Cell> cells = new LinkedList<Cell>();
    private List<Line> lines = new List<Line>();
    private List<Vector3> positions = new List<Vector3>();

    public override void DoTutorial()
    {
        var highestCell = GridManager.Instance.allCell.First(x => x.Value == GameSystem.userdata.gameData.currentHighestCellValue);
        cells.AddFirst(highestCell);
        for (int i = 0; i < Paint.MAX_CELL - 1; i++)
        {
            var cell = cells.Last.Value.nearbyCell.First(x => !cells.Contains(x));
            if (cell != null) cells.AddAfter(cells.Last, cell);
        }
        GetPositions(cells.First);
        ConectCells(cells.First);
        StartCoroutine(TutorialHandMove(positions.ToArray()));
    }

    private void GetPositions(LinkedListNode<Cell> startPoint)
    {
        positions.Add(startPoint.Value.transform.position);
        if (!startPoint.Equals(cells.Last)) GetPositions(startPoint.Next);
    }

    private void ConectCells(LinkedListNode<Cell> startPoint)
    {
        if (startPoint.Next == null) return;
        var line = PoolSystem.Instance.GetObject(linePrefab, Vector3.zero);
        line.SetLine(startPoint.Value, startPoint.Next.Value);
        line.SetColor(startPoint.Value.spriteRenderer.color);
        lines.Add(line);
        ConectCells(startPoint.Next);
    }

    private IEnumerator TutorialHandMove(Vector3[] positions)
    {
        hand.sprite = pressSprite;
        hand.transform.position = positions[0];
        hand.transform.DOPath(positions, 2f).OnComplete(() =>
        {
            hand.transform.DOMove(positions[0], 2f);
            hand.sprite = releaseSprite; 
        }) ;
        yield return new WaitForSeconds(4f);
        StartCoroutine(TutorialHandMove(positions));
    }

    private void ClearData()
    {
        cells.Clear();
        positions.Clear();
        foreach (var item in lines)
        {
            item.gameObject.SetActive(false);
        }
        lines.Clear();
    }

    private void OnDisable()
    {
        ClearData();
    }
}
