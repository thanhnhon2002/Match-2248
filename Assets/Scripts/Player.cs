using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using UnityEngine;
using Vector3 = UnityEngine.Vector3;

public class Player : MonoBehaviour
{
    public static Player Instance { get; private set; }
    public Vector3 mousePos { get; private set; }
    [SerializeField] private Line linePrefab;
    [SerializeField] private Effect effectPrefab;
    private List<Cell> conectedCell = new List<Cell>();
    public List<Cell> ConectedCell =>conectedCell;
    private List<Line> lines = new List<Line> ();
    private Dictionary<BigInteger, int> conectedValueCount = new Dictionary<BigInteger, int> ();
    private Camera mainCam;
    public bool isDraging;
    private BigInteger segmentCount;
    private BigInteger currentCellValue;
    private BigInteger initValue;
    private int countInitValue;

    private void Awake ()
    {
        Instance = this;
        mainCam = Camera.main;
    }

    private void Update ()
    {
        mousePos = mainCam.ScreenToWorldPoint (Input.mousePosition);
        if (lines.Count > 0)
        {
            var lastLine = lines[lines.Count - 1];
            lastLine.SetLine (lastLine.startCell, null);
        }
    }

    public void InitLine (Cell cell)
    {
        if (GameFlow.Instance.gameState != GameState.Playing) return;
        if (conectedCell.Contains (cell)) return;
        Debug.Log ("Init");
        cell.OnInteract?.Invoke ();
        currentCellValue = cell.Value;
        initValue = cell.Value;
        if (!conectedValueCount.ContainsKey (cell.Value)) conectedValueCount.Add (cell.Value, 0);
        conectedValueCount[cell.Value] = 1;
        conectedCell.Add (cell);
        segmentCount++;
        GameFlow.Instance.TotalPoint = currentCellValue;
        var line = PoolSystem.Instance.GetObject (linePrefab, cell.transform.position);
        lines.Add (line);
        line.SetLine (cell, null);
        line.SetColor (cell.spriteRenderer.color);
        isDraging = true;
    }

    public void CheckCell(Cell cell)
    {
        if (GameFlow.Instance.gameState != GameState.Playing) return;
        if (!isDraging) return;
        Debug.Log ("Check");
        if (conectedCell.Contains (cell) && conectedCell.Count > 1 && cell.Equals (conectedCell[conectedCell.Count - 2]))
        {
            RemoveCell(conectedCell[conectedCell.Count - 1]);
            currentCellValue = conectedCell[conectedCell.Count - 1].Value;
        }
        else if (!conectedCell.Contains(cell)) AddCell(cell);
    }

    public void AddCell(Cell cell)
    {
        if (GameFlow.Instance.gameState != GameState.Playing) return;
        var lastCell = conectedCell[conectedCell.Count - 1];
        if (!lastCell.nearbyCell.Contains (cell)) return;
        if (!CanConect (cell)) return;
        cell.OnInteract?.Invoke ();

        //conect last line to cell
        var lastLine = lines[lines.Count - 1];
        lastLine.SetLine (lastLine.startCell, cell);

        //spawn new line and conect it with mouse position
        var line = PoolSystem.Instance.GetObject (linePrefab, cell.transform.position);
        lines.Add (line);
        line.SetLine (cell, null);
        line.SetColor (cell.spriteRenderer.color);

        conectedCell.Add (cell);
        segmentCount += cell.Value / currentCellValue;
        countInitValue += (int)(cell.Value /initValue);
        
        if (countInitValue >= 1) GameFlow.Instance.CalculateTotal (initValue, countInitValue);
    }

    public void RemoveCell(Cell lastCell)
    {
        if (GameFlow.Instance.gameState != GameState.Playing) return;
        if (!isDraging) return;
        if (conectedCell.Count <= 1) return;
        var lastLine = lines[lines.Count - 1];
        lastLine.gameObject.SetActive (false);
        lines.Remove (lastLine);
        lastCell.OnInteract?.Invoke ();
        var line = lines.Find (x => x.endCell.Equals (lastCell));
        line.SetLine(line.startCell, null);

        segmentCount -= lastCell.Value / currentCellValue;
        countInitValue -= (int)(lastCell.Value / initValue);

        conectedCell.Remove (lastCell);
        conectedValueCount[lastCell.Value]--;
        if (conectedValueCount[lastCell.Value] == 0) conectedValueCount.Remove (lastCell.Value);
        GameFlow.Instance.CalculateTotal (initValue, countInitValue);
    }

    private bool CanConect(Cell cell)
    {       
        if (!conectedValueCount.ContainsKey(cell.Value / 2)&&cell.Value>initValue) return false;
        if (cell.Value < currentCellValue) return false;
        else if (currentCellValue == cell.Value)
        {
            conectedValueCount[cell.Value]++;
            return true;
        }
        else
        {
            if(cell.Value > GameFlow.Instance.TotalPoint) return false;
            currentCellValue = cell.Value;
            conectedValueCount.Add (cell.Value, 1);
            return true;
        }
    }
    public void ClearLine ()
    {
        foreach (var line in lines)
        {
            line.gameObject.SetActive (false);
        }
        if (conectedCell.Count >= 2) ExploseConectedCell ();
        else ResetData ();
    }

    private void ResetData ()
    {
        lines.Clear ();
        conectedValueCount.Clear ();
        conectedCell.Clear ();
        isDraging = false;
        segmentCount = 0;
        countInitValue = 0;      
    }

    private void ExploseConectedCell ()
    {
        var effectTime = 0f;
        var lastCell = conectedCell.Last () ;
        var newValue = GameFlow.Instance.TotalPoint;
        var newColor = GridManager.Instance.GetCellColor (newValue);
        for (int i = 0; i < conectedCell.Count; i++)
        {
            if (!conectedCell[i].Equals (lastCell)) conectedCell[i].gameObject.SetActive (false);
            else
            {
                var color = lastCell.spriteRenderer.color;
                color.a = 0;
                lastCell.spriteRenderer.color = color;
                lastCell.valueTxt.color = color;
            }
            GameFlow.Instance.gameState = GameState.Fx;
            var fx = PoolSystem.Instance.GetObject (effectPrefab, conectedCell[i].transform.position);
            fx.Play (conectedCell, i, conectedCell[i].spriteRenderer.color, newColor);
            effectTime = fx.time;
        }

        LeanTween.delayedCall (effectTime, () =>
        {
            newColor.a = 1;
            var textColor = Color.white;
            textColor.a = 1;
            lastCell.spriteRenderer.color = newColor;
            lastCell.Value = newValue;
            lastCell.valueTxt.color = textColor;
            LeanTween.delayedCall (effectTime, () => 
            {
                GameFlow.Instance.AddScore ();
                GridManager.Instance.CheckToSpawnNewCell (conectedCell);
                ResetData ();
            });
        });
    }
    
}
