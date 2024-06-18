using DG.Tweening;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Hint : MonoBehaviour
{
    private List<Cell> allCells = new List<Cell>();
    private List<Cell> hintcells = new List<Cell>();
    private List<Line> lines = new List<Line>();
    private const float TIME_SHOW_HINT = 5;
    [SerializeField] private Line linePrefab;
    public static Hint Instance;
    public float timeLastDrag;
    const float TIME_FADE_HINT = 1.2f;
    Sequence sequence;
    private void Awake()
    {
        Instance = this;
    }

    private void Update()
    {
        if (Time.time > timeLastDrag + TIME_SHOW_HINT && timeLastDrag != 0)
        {
            FindHint();
            timeLastDrag = 0;
        }
    }
    public void StartHint()
    {
       if( GameFlow.Instance.gameState == GameState.Playing) timeLastDrag = Time.time;
    }
    void InintHint()
    {
        sequence = DOTween.Sequence();
        ClearList();
        allCells.AddRange(GridManager.Instance.allCell);
        //allCells.Sort((a, b) => a.Value.CompareTo(b.Value));
    }
    [ContextMenu("Hint")]
    public void FindHint()
    {
        this.InintHint();
        foreach (var cell in allCells)
        {
            if (cell.ConectableCount > 0)
            {
                hintcells.Add(cell);
                FindCellHintNear(cell);
                break;
            }
        }
        AnimationHint();
    }
    void FindCellHintNear(Cell cell)
    {
        foreach(var cellNear in cell.nearbyCell)
        {
            //if (cellNear.Value == hintcells.Last().Value && !hintcells.Contains(cellNear))
            if (CanConect(cellNear) && !hintcells.Contains(cellNear))
            {
                ShowHint(cellNear);
                hintcells.Add(cellNear);
                FindCellHintNear(cellNear);
                break;
            }
        }
    }
    private bool CanConect(Cell cell)
    {
        if (cell.Value == hintcells.Last().Value) return true;
        if(hintcells.Count>2 && cell.Value/2 == hintcells.Last().Value) return true;
        return false;
    }
    void ShowHint(Cell cell)
    {
        var line = PoolSystem.Instance.GetObject(linePrefab, hintcells.Last().transform.position);
        lines.Add(line);
        line.SetLine(hintcells.Last(), cell);
    }
    LineRenderer lineRenderer;
    Color colorStart;
    Color colorEnd;
    SpriteRenderer sprite;
    Color color;
    void AnimationHint()
    {
        sequence = DOTween.Sequence();
        sequence.AppendCallback(() =>
        {
            foreach (var cell in hintcells)
            {

                if (cell != hintcells.Last())
                {
                    lineRenderer = lines[hintcells.IndexOf(cell)].GetComponent<LineRenderer>();
                    colorStart = cell.spriteRenderer.color;
                    colorStart.a = 0.2f;
                    lines[hintcells.IndexOf(cell)].SetColor(colorStart);
                    colorEnd = cell.spriteRenderer.color;
                    colorEnd.a = 1f;
                    lineRenderer.DOColor(new Color2(colorStart, colorStart), new Color2(colorEnd, colorEnd), TIME_FADE_HINT);
                }
                cell.highLight.SetActive(true);
                var sprite = cell.highLight.GetComponent<SpriteRenderer>();
                color = sprite.color;
                color.a = 0.05f;
                sprite.color = color;
                sprite.DOFade(0.7f, TIME_FADE_HINT);

            }
        });
        sequence.AppendInterval(TIME_FADE_HINT);
        sequence.AppendCallback(() =>
        {
            foreach (var cell in hintcells)
            {
                if (cell != hintcells.Last())
                {
                    lineRenderer = lines[hintcells.IndexOf(cell)].GetComponent<LineRenderer>();
                    colorStart = cell.spriteRenderer.color;
                    colorStart.a = 0.2f;
                    lines[hintcells.IndexOf(cell)].SetColor(colorStart);
                    colorEnd = cell.spriteRenderer.color;
                    colorEnd.a = 1f;
                    lineRenderer.DOColor(new Color2(colorEnd, colorEnd), new Color2(colorStart, colorStart), TIME_FADE_HINT);
                }               
                cell.highLight.SetActive(true);
                var sprite = cell.highLight.GetComponent<SpriteRenderer>();
                sprite.DOFade(0.05f, TIME_FADE_HINT);
            }
        });
        sequence.AppendInterval(TIME_FADE_HINT);
        sequence.SetLoops(-1);
    }
    public void ClearList()
    {
        allCells.Clear();
        foreach (var cell in hintcells)
        {
            sprite = cell.highLight.GetComponent<SpriteRenderer>();           
            color = sprite.color;
            color.a = (float) 155/255;
            sprite.color = color;
            cell.highLight.SetActive(false);
            DOTween.Kill(sprite);
        }
        hintcells.Clear();
        foreach (var line in lines)
        {
            DOTween.Kill(line.GetComponent<LineRenderer>());
            line.gameObject.SetActive(false);
        };
        lines.Clear();
        sequence.Kill();
    }
}