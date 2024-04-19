using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using DG.Tweening;
using System.Numerics;
using Random = UnityEngine.Random;
using Vector2 = UnityEngine.Vector2;
using Vector3 = UnityEngine.Vector3;
using Quaternion = UnityEngine.Quaternion;

public class GridManager : MonoBehaviour
{
    private const float CELL_DROP_TIME = 0.5F;
    public readonly int MAX_ROW = 8;
    public readonly int MAX_COL = 5;
    public static GridManager Instance { get; private set; }
    public static readonly List<GridPosition> neighbourGridPosition = new List<GridPosition> ()
    { new GridPosition(0, 1), new GridPosition (1,1), new GridPosition(1, 0), new GridPosition(1,-1), new GridPosition(0, -1), new GridPosition(-1, -1), new GridPosition(-1, 0), new GridPosition(-1,1) };
    [SerializeField] private Cell cellPrefab;
    [SerializeField] private Transform cellSpawnPos;
    private Dictionary<int, List<Cell>> allCellInCollom = new Dictionary<int, List<Cell>>();
    private Dictionary<GridPosition, Vector3> girdPosToLocal = new Dictionary<GridPosition, Vector3> ();
    private Dictionary<Cell, int> spawnInfo = new Dictionary<Cell, int> ();
    private List<Cell> conectedCellInCollom = new List<Cell> ();
    private List<Cell> existedCellInCollom = new List<Cell> ();

    private Cell[] allCell;
    public Dictionary<GridPosition, Cell> cellDic = new Dictionary<GridPosition, Cell> ();

    private const int Space_Index = 10;
    private const int Space_MaxIndex = 13;
    private int minIndex;
    private int maxIndex;
    private int maxIndexRandom;


    private void Awake ()
    {
        DOTween.SetTweensCapacity (1500, 1500);
        Application.targetFrameRate = 60;
        Instance = this;
    }

    private void Start ()
    {
        minIndex = 1;
        maxIndex = Space_Index;
        maxIndexRandom = (int)(maxIndex + minIndex) / 2;
        allCell = GetComponentsInChildren<Cell> ();
        foreach (var item in allCell)
        {
            girdPosToLocal.Add (item.gridPosition, item.transform.localPosition);
        }
        UpdateCell ();
        LoadCells ();
    }

    private void UpdateCell ()
    {
        allCell = GetComponentsInChildren<Cell> ();
        cellDic.Clear ();
        foreach (var item in allCell)
        {
            cellDic.Add (item.gridPosition, item);
            if(!allCellInCollom.ContainsKey(item.gridPosition.x)) allCellInCollom.Add(item.gridPosition.x, new List<Cell> ());
            allCellInCollom[item.gridPosition.x].Add(item);
        }
        foreach (var item in allCell)
        {
            item.FindNearbyCells ();
        }
    }

    private void LoadCells ()
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

    public void CheckToSpawnNewCell (List<Cell> conectedCell)
    {
        for (int i = 1; i <= MAX_COL; i++)
        {
            var list = allCellInCollom[i];
            var spawnPos = new Vector2 (list[i].transform.localPosition.x, cellSpawnPos.transform.position.y);
            list.RemoveAll (x => !x.gameObject.activeInHierarchy);
            var count = conectedCell.Count (x => x.gridPosition.x == i && !x.Equals (conectedCell.Last ()));
            for (int j = 0; j < count; j++)
            {
                var newCell = SpawnCell (spawnPos, (int)Mathf.Pow (2, Random.Range (1, 7)));
                spawnPos.y++;
                if (!list.Contains (newCell)) list.Add (newCell);
            }
            list = list.Distinct ().ToList ();
            list.Sort ((a, b) => a.transform.localPosition.y.CompareTo (b.transform.localPosition.y));
            Debug.Log ($"Collom {i} has {list.Count} cells");
            var gridY = list.Count;
            for (int a = 0; a < list.Count; a++)
            {
                list[a].gameObject.SetActive (true);
                list[a].gridPosition = new GridPosition (i, gridY);
                gridY--;
            }
        }
        Drop ();
    }

    public void Drop()
    {
        for (int i = 1; i <= MAX_COL; i++)
        {
            var list = allCellInCollom[i];
            foreach (var item in list)
            {
                //item.transform.localPosition = girdPosToLocal[item.gridPosition];
                item.transform.DOLocalMoveY (girdPosToLocal[item.gridPosition].y, CELL_DROP_TIME);
                item.colorSet.SetColor ();
            }
        }
        LeanTween.delayedCall (CELL_DROP_TIME,UpdateCell);
    }
    public Cell SpawnCell (Vector2 position, int value)
    {
        var cell = PoolSystem.Instance.GetObject (cellPrefab, position);
        cell.transform.SetParent (transform);
        cell.Value = value;
        return cell;
    }

    public Cell GetCellAt (GridPosition position)
    {
        if (!cellDic.ContainsKey (position)) return null;
        return cellDic[position];
    }
    public Cell GetCellAt (GridPosition position, out bool outOfBound)
    {
        if (!cellDic.ContainsKey (position))
        {
            outOfBound = true;
            return null;
        }
        outOfBound = false;
        return cellDic[position];
    }

    [ContextMenu ("Spawn")]
    public void SpawnGrid ()
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
