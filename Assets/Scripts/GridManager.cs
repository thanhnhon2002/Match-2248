using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UnityEngine.UIElements;

public class GridManager : MonoBehaviour
{
    public readonly int MAX_ROW = 8;
    public readonly int MAX_COL = 5;
    public static GridManager Instance { get; private set; }
    public static readonly List<GridPosition> neighbourGridPosition = new List<GridPosition> ()
    { new GridPosition(0, 1), new GridPosition (1,1), new GridPosition(1, 0), new GridPosition(1,-1), new GridPosition(0, -1), new GridPosition(-1, -1), new GridPosition(-1, 0), new GridPosition(-1,1) };
    [SerializeField] private Cell cellPrefab;

    private Cell[] allCell;
    public Dictionary<GridPosition, Cell> cellDic = new Dictionary<GridPosition, Cell> ();

    private int minIndex;
    private int maxIndex;
    private int maxIndexRandom;


    private void Awake ()
    {
        Application.targetFrameRate = 60;
        Instance = this;
        UpdateCell ();
        LoadCells ();
    }
    private void Start()
    {
        minIndex = 1;
        maxIndex = 10;
        maxIndexRandom = (int)(maxIndex + minIndex) / 2;
    }
    private void UpdateCell ()
    {
        allCell = GetComponentsInChildren<Cell> ();
        foreach (var item in allCell)
        {
            cellDic.Add (item.gridPosition, item);
        }
        foreach (var item in allCell)
        {
            item.FindNearbyCells ();
        }
    }

    private void LoadCells()
    {
        foreach (var item in allCell)
        {
            item.Value = (int)Mathf.Pow (2, Random.Range (1, 7));
        }
    }

    public void SpawnNewCell(Vector2 position, int value)
    {
        var newCell = PoolSystem.Instance.GetObject (cellPrefab, position);
        newCell.Value = value;
        int index = (int)Mathf.Log(value, 2);
        if(index>maxIndex)
        {
            minIndex++;
            maxIndex++;
            maxIndexRandom++;
        }
    }

    public Cell GetCellAt(GridPosition position)
    {
        if (!cellDic.ContainsKey (position)) return null;
        return cellDic[position];
    }

    [ContextMenu("Spawn")]
    public void SpawnGrid()
    {
        var pos = new Vector2 ((int)(-MAX_COL / 2f), (MAX_ROW / 2f) - 0.5f);
        var gridPos = new GridPosition (1, 1);
        for (int x = 0; x < MAX_COL; x++)
        {
            for (int y = 0; y < MAX_ROW; y++)
            {
                var cell = Instantiate (cellPrefab, pos, Quaternion.identity);
                cell.gridPosition = gridPos;
                gridPos.y += 1;
                pos.y -= 1f;
            }
            gridPos.x += 1;
            gridPos.y = 1;
            pos.x += 1f;
            pos.y = (MAX_ROW / 2f) - 0.5f;
        }
    }
    public int ValueRandom()
    {
        int rad = Random.Range(minIndex, maxIndexRandom+1);
        return (int)Mathf.Pow(2, rad);
    }
}
