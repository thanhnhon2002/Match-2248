using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UnityEngine.UIElements;
using Unity.Mathematics;
using System.Numerics;
using Random = UnityEngine.Random;
using Vector2 = UnityEngine.Vector2;
using Quaternion = UnityEngine.Quaternion;

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

    private const int Space_Index = 10;
    private const int Space_MaxIndex = 13;
    private int minIndex;
    private int maxIndex;
    private int maxIndexRandom;


    private void Awake ()
    {
        Application.targetFrameRate = 60;
        Instance = this;
    }
    private void Start()
    {
        minIndex = 1;
        maxIndex = Space_Index;
        maxIndexRandom = (int)(maxIndex + minIndex) / 2;
        UpdateCell();
        LoadCells();
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
            item.Value = (BigInteger)Mathf.Pow(2, Random.Range(1, 5));
        }
    }

    public void SpawnCellSum(Vector2 position, BigInteger value)
    {
        var newCell = PoolSystem.Instance.GetObject (cellPrefab, position);
        newCell.Value = value;
        Mathf mathf;
        int index = mathf.LogBigInt(value,2);
        if (index > maxIndex)
        {
            minIndex++;
            if (maxIndex - minIndex < Space_MaxIndex) maxIndex += 2;
            else maxIndex += 1;
            maxIndexRandom++;
        }
        GameFlow.Instance.TotalPoint = 0;
    }
    public void SpawnCellNew(UnityEngine.Vector2 position)
    {
        var newCell = PoolSystem.Instance.GetObject(cellPrefab, position);
        newCell.Value = ValueRandom();
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
    public BigInteger ValueRandom()
    {
        int index = Random.Range(minIndex, maxIndexRandom+1);
        return (BigInteger)Mathf.Pow(2, index);
    }
}
