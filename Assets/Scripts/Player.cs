using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using static UnityEditor.Progress;

public class Player : MonoBehaviour
{
    public static Player Instance { get; private set; }
    public Vector3 mousePos { get; private set; }
    [SerializeField] private Line linePrefab;
    [SerializeField] private Effect effectPrefab;
    private List<Cell> conectedCell = new List<Cell> ();
    private List<Line> lines = new List<Line> ();
    private Dictionary<int, int> conectedValueCount = new Dictionary<int, int> ();
    private Camera mainCam;
    public bool isDraging;
    private int segmentCount;
    private int initValue;

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
        if (conectedCell.Contains (cell)) return;
        cell.OnInteract?.Invoke ();
        initValue = cell.Value;
        if (!conectedValueCount.ContainsKey (cell.Value)) conectedValueCount.Add (cell.Value, 0);
        conectedValueCount[cell.Value] = 1;
        conectedCell.Add (cell);
        segmentCount++;
        GameFlow.Instance.TotalPoint = initValue;
        var line = PoolSystem.Instance.GetObject (linePrefab, cell.transform.position);
        lines.Add (line);
        line.SetLine (cell, null);
        line.SetColor (cell.spriteRenderer.color);
        isDraging = true;
    }

    public void CheckCell(Cell cell)
    {
        if (!isDraging) return;
        if (conectedCell.Contains (cell) && conectedCell.Count > 1 && cell.Equals (conectedCell[conectedCell.Count - 2]))
        {
            RemoveCell (conectedCell[conectedCell.Count - 1]);
            initValue = conectedCell[conectedCell.Count - 1].Value;
        } else if (!conectedCell.Contains (cell)) AddCell (cell);
    }

    public void AddCell(Cell cell)
    {
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
        segmentCount += cell.Value / initValue;
        if (segmentCount >= 2) GameFlow.Instance.CalculateTotal (initValue, segmentCount);
    }

    public void RemoveCell(Cell lastCell)
    {
        if (!isDraging) return;
        if (conectedCell.Count <= 1) return;
        var lastLine = lines[lines.Count - 1];
        lastLine.gameObject.SetActive (false);
        lines.Remove (lastLine);
        lastCell.OnInteract?.Invoke ();
        var line = lines.Find (x => x.endCell.Equals (lastCell));
        line.SetLine(line.startCell, null);
        segmentCount -= lastCell.Value / initValue;
        conectedCell.Remove (lastCell);
        conectedValueCount[lastCell.Value]--;
        if (conectedValueCount[lastCell.Value] == 0) conectedValueCount.Remove (lastCell.Value);
        if (segmentCount >= 2) GameFlow.Instance.CalculateTotal (initValue, segmentCount);
    }

    private bool CanConect(Cell cell)
    {
        if (conectedValueCount.ContainsKey (cell.Value) && initValue == cell.Value)
        {
            conectedValueCount[cell.Value]++;
            return true;
        }
        if (!conectedValueCount.ContainsKey (cell.Value / 2)) return false;
        var valueCount = conectedValueCount[cell.Value / 2];
        if (cell.Value > initValue && !conectedValueCount.ContainsKey(cell.Value) && valueCount >= 1/* && Mathf.Log (cell.Value, 2) >= valueCount*/ && conectedCell.Count > 1)
        {
            initValue = cell.Value;
            if (!conectedValueCount.ContainsKey (cell.Value))
            {
                conectedValueCount.Add (cell.Value, 1);
            } else conectedValueCount[cell.Value]++;
            return true;
        }
        return false;
    }

    public void ClearLine ()
    {
        foreach (var line in lines)
        {
            line.gameObject.SetActive (false);
        }
        for (int i = 0; i < conectedCell.Count; i++)
        {
            conectedCell[i].gameObject.SetActive (false);
            var fx = PoolSystem.Instance.GetObject (effectPrefab, conectedCell[i].transform.position);
            fx.Play (conectedCell, i);
        }
        lines.Clear ();
        conectedValueCount.Clear ();
        conectedCell.Clear ();
        isDraging = false;
        segmentCount = 0;
    }
}
