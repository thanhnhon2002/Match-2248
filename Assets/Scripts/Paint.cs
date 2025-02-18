using DG.Tweening;
using NSubstitute.Core;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using UnityEngine;
using UnityEngine.UI;
using Vector3 = UnityEngine.Vector3;

public class Paint : Power<Paint>
{
    public const int MAX_CELL = 5;
    [SerializeField] private Line linePrefab;
    [SerializeField] private PaintTutorial tutorialPrefab;
    private PaintTutorial turorial;
    private List<Cell> chosenCells = new List<Cell>();
    private List<Line> lines = new List<Line>();
    private bool isDraging;

    public override void UsePower()
    {
        if (GameFlow.Instance.gameState != GameState.Playing) return;
        if (GameSystem.userdata.diamond < cost)
        {
            GameFlow.Instance.shop.SetActive(true);
            GameFlow.Instance.shop.GetComponent<EffectOpenSlide>().DoEffect();
            return;
        }
        base.UsePower();
        if (!info.isTutorialFinish)
        {
            turorial = Instantiate(tutorialPrefab, Vector3.zero, UnityEngine.Quaternion.identity);
            turorial.DoTutorial();
            backButton.gameObject.SetActive(false);
        }
        GameFlow.Instance.gameState = GameState.Paint;
        chosenCells.Clear();
        lines.Clear();
    }


    public override void UsePowerIgnoreCost()
    {
        base.UsePowerIgnoreCost();
        base.UsePower();
        GameFlow.Instance.gameState = GameState.Paint;
        chosenCells.Clear();
        lines.Clear();
    }

    public void BeginPaint(Cell cell)
    {
        if (chosenCells.Contains(cell)) return;
        cell.OnInteract?.Invoke();
        chosenCells.Add(cell);
        var line = PoolSystem.Instance.GetObject(linePrefab, cell.transform.position);
        lines.Add(line);
        line.SetLine(cell, null);
        line.SetColor(cell.spriteRenderer.color);
        isDraging = true;
    }

    public void CheckCell(Cell cell)
    {
        if (!isDraging) return;
        if (chosenCells.Contains(cell) && chosenCells.Count > 1 && cell.Equals(chosenCells[chosenCells.Count - 2]))
            RemoveCell(chosenCells[chosenCells.Count - 1]);
        else if (!chosenCells.Contains(cell)) ConectCell(cell);
    }

    public void PaintAllCells()
    {
        if (turorial != null) turorial.gameObject.SetActive(false);
        isDraging = false;
        ClearAllLines();
        Back();
        if (chosenCells.Count == 1)
        {
            //GameFlow.Instance.gameState = GameState.Playing;
            //if (turorial != null) turorial.gameObject.SetActive(true);
            return;
        }
        info.isTutorialFinish = true;
        var highestValue = FindHighestValue();
        GameFlow.Instance.gameState = GameState.Fx;
        var sq = DOTween.Sequence();
        sq.AppendCallback(() =>
        {
            foreach (var item in chosenCells)
            {
                item.IncreaseValue(highestValue);
            }
        });
        sq.AppendInterval(Const.DEFAULT_TWEEN_TIME + 0.05f);
        sq.AppendCallback(() =>
        {
            foreach (var item in chosenCells)
            {
                item.Value = highestValue;
            }
            GameFlow.Instance.gameState = GameState.Playing;
            GridManager.Instance.UpdateCell(true);
            onUseCompleted?.Invoke();
            DisplayCost();
        });
    }

    private BigInteger FindHighestValue()
    {
        BigInteger highestValue = 0;
        foreach (var item in chosenCells)
        {
            if (item.Value > highestValue) highestValue = item.Value;
        }
        return highestValue;
    }

    private void ClearAllLines()
    {
        foreach (var item in lines)
        {
            item.gameObject.SetActive(false);
        }
        lines.Clear();
    }

    private void ConectCell(Cell cell)
    {
        if (chosenCells.Contains(cell) || chosenCells.Count >= MAX_CELL) return;
        var lastCell = chosenCells.Last();
        if (!lastCell.nearbyCell.Contains(cell)) return;

        chosenCells.Add(cell);
        cell.OnInteract?.Invoke();

        var lastLine = lines[lines.Count - 1];
        lastLine.SetLine(lastLine.startCell, cell);

        var line = PoolSystem.Instance.GetObject(linePrefab, cell.transform.position);
        lines.Add(line);
        line.SetLine(cell, null);
        line.SetColor(cell.spriteRenderer.color);
    }


    private void RemoveCell(Cell cell)
    {
        chosenCells.Remove(cell);
        var lastLine = lines[lines.Count - 1];
        lastLine.gameObject.SetActive(false);
        lines.Remove(lastLine);
        cell.OnInteract?.Invoke();
        var line = lines.Find(x => x.endCell.Equals(cell));
        line.SetLine(line.startCell, null);
    }
}
