using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using DG.Tweening;
using System.Numerics;
using Random = UnityEngine.Random;
using Vector2 = UnityEngine.Vector2;
using Vector3 = UnityEngine.Vector3;
using Quaternion = UnityEngine.Quaternion;
using System;
using System.Collections;
using DarkcupGames;

public class GridManager : MonoBehaviour
{
    private const float CELL_DROP_TIME = 0.5F;
    public readonly int MAX_ROW = 8;
    public readonly int MAX_COL = 5;
    private const int Space_Index = 10;
    private const int Space_MaxIndex = 13;

    public static GridManager Instance { get; private set; }
    public static readonly List<GridPosition> neighbourGridPosition = new List<GridPosition>()
    { new GridPosition(0, 1), new GridPosition (1,1), new GridPosition(1, 0), new GridPosition(1,-1), new GridPosition(0, -1), new GridPosition(-1, -1), new GridPosition(-1, 0), new GridPosition(-1,1) };
    [SerializeField] private Cell cellPrefab;
    [SerializeField] private Transform cellSpawnPos;
    [SerializeField] private ParticalSystemController removeCellFx;
    public Dictionary<int, List<Cell>> allCellInCollom = new Dictionary<int, List<Cell>>();
    private Dictionary<GridPosition, Vector3> girdPosToLocal = new Dictionary<GridPosition, Vector3>();
    public Cell[] allCell { get; private set; }
    public Dictionary<GridPosition, Cell> cellDic = new Dictionary<GridPosition, Cell>();

    private int minIndex;
    public int MinIndex => minIndex;
    private int maxIndex;
    public int MaxIndex => maxIndex;
    private int maxIndexRandom;
    public int MaxIndexRandom => maxIndexRandom;
    private int indexPlayer;
    public int IndexPlayer => indexPlayer;
    public int indexStart;
    public int indexChose;

    private List<Cell> lowestCells = new List<Cell>();

    //Only use to check Conected Cell
    private List<Cell> cellCol1 = new List<Cell>();
    private List<Cell> cellCol2 = new List<Cell>();
    private List<Cell> cellCol3 = new List<Cell>();
    private List<Cell> cellCol4 = new List<Cell>();
    private List<Cell> cellCol5 = new List<Cell>();

    //[SerializeField] private List<Cell> debugCellCol1 = new List<Cell> ();
    //[SerializeField] private List<Cell> debugCellCol2 = new List<Cell> ();
    //[SerializeField] private List<Cell> debugCellCol3 = new List<Cell> ();
    //[SerializeField] private List<Cell> debugCellCol4 = new List<Cell> ();
    //[SerializeField] private List<Cell> debugCellCol5 = new List<Cell> ();


    private void Awake()
    {
        DOTween.SetTweensCapacity(1500, 1500);
        Application.targetFrameRate = 60;
        Instance = this;
    }

    private void Start()
    {
        allCell = GetComponentsInChildren<Cell>();
        var userData = GameSystem.userdata;
        if (userData.replay)
        {
            foreach (var item in allCell)
            {
                item.gameObject.SetActive(false);
            }
            //StartCoroutine(WaitChoseStart());
            return;
        }
        SetUpCell();
        FirebaseManager.Instance.SetProperty(UserPopertyKey.last_level, maxIndex.ToString());
    }
    void SetUpCell()
    {
        var userData = GameSystem.userdata;
        if (userData.gameData.minIndex == 0)
        {
            minIndex = 1;
            maxIndex = Space_Index + minIndex - 1;
            maxIndexRandom = (int)(maxIndex + minIndex) / 2 + 2;
            indexPlayer = maxIndexRandom;
        } else LoadDataIndex();
        foreach (var item in allCell)
        {
            girdPosToLocal.Add(item.gridPosition, item.transform.localPosition);
        }
        UpdateCell();
        LoadCells();
    }
    //IEnumerator WaitChoseStart()
    //{
    //    while(indexStart==0)
    //    {
    //        yield return null;
    //    }
    //    SetUpCell();
    //    indexChose = 0;
    //    indexStart = 0;
    //    GameSystem.userdata.replay = false;
    //    GameSystem.SaveUserDataToLocal();
    //}
    public void SetStartIndex()
    {
        foreach (var item in allCell)
        {
            item.gameObject.SetActive(true);
        }
        indexStart = indexChose;
        SetUpCell();
        var startValue = (BigInteger)Mathf.Pow(2, indexStart);
        allCell.RandomElement().Value = startValue;
        FirebaseManager.Instance.SetProperty(UserPopertyKey.last_level, indexChose.ToString());
        SetSumValue(startValue);
        var userData = GameSystem.userdata;
        userData.gameData.currentHighestCellValue = startValue;
        indexChose = 0;
        indexStart = 0;
        userData.replay = false;
        GameSystem.SaveUserDataToLocal();
    }
    public void SetIndexChose(int index)
    {
        indexChose = index;
    }
    public void LoadDataIndex()
    {
        var userData = GameSystem.userdata;
        indexPlayer = userData.gameData.indexPlayer;
        minIndex = userData.gameData.minIndex;
        maxIndex = userData.gameData.maxIndex;
        maxIndexRandom = userData.gameData.maxIndexRandom;
    }

    public Cell SpawnCell(Vector2 position, int value)
    {
        var cell = PoolSystem.Instance.GetObject(cellPrefab, position);
        //var cell = Instantiate(cellPrefab, position, Quaternion.identity);
        cell.gridPosition = new GridPosition(0, 0);
        cell.transform.SetParent(transform);
        cell.Value = value;
        return cell;
    }

    public Cell GetCellAt(GridPosition position)
    {
        if (!cellDic.ContainsKey(position)) return null;
        return cellDic[position];
    }

    public BigInteger ValueRandom()
    {
        int index = Random.Range(minIndex, maxIndexRandom + 1);
        return (BigInteger)Mathf.Pow(2, index);
    }

    public void SetSumValue(BigInteger value)
    {
        Mathf mathf;
        int index = mathf.LogBigInt(value, 2);
        if (index > indexPlayer)
        {
            indexPlayer = index;
            Debug.Log("New Block 2^" + indexPlayer);
            FirebaseManager.Instance.LogLevelPass(index, GameFlow.Instance.timeCount);
            PopupManager.Instance.SubShowPopup(new DataEventPopup(PopupManager.Instance.ShowPopup, PopupOptions.NewBlock));
            PopupManager.Instance.SubShowPopup(new DataEventPopup(PopupManager.Instance.ShowPopup, PopupOptions.Duplicate));
        }
        if (index > maxIndex)
        {
            GetLowestCells();
            Debug.Log("Lock 2^" + minIndex);
            minIndex++;
            if (maxIndex - minIndex < Space_MaxIndex) maxIndex += 2;
            else maxIndex += 1;
            maxIndexRandom++;
            Debug.Log("AddBlock 2^" + maxIndexRandom);

            PopupManager.Instance.SubShowPopup(new DataEventPopup(PopupManager.Instance.ShowPopup, PopupOptions.LockElinimated));
            PopupManager.Instance.SubShowPopup(new DataEventPopup(PopupManager.Instance.ShowPopup, PopupOptions.BlockAdded));

        }
        var userData = GameSystem.userdata;
        userData.gameData.indexPlayer = indexPlayer;
        userData.gameData.minIndex = minIndex;
        userData.gameData.maxIndex = maxIndex;
        userData.gameData.maxIndexRandom = maxIndexRandom;
        GameSystem.SaveUserDataToLocal();
        GameFlow.Instance.TotalPoint = 0;
    }


    public Cell SpawnCellNew(Vector2 position)
    {
        var newCell = PoolSystem.Instance.GetObject(cellPrefab, position);
        newCell.gridPosition = new GridPosition(0, 0);
        newCell.transform.SetParent(transform);
        newCell.Value = ValueRandom();
        return newCell;
    }

    public void CheckToSpawnNewCell(List<Cell> conectedCell)
    {
        CollectConectedCell(conectedCell);

        for (int i = 1; i <= MAX_COL; i++)
        {
            var list = RecollectActiveCell(i);
            List<Cell> checkList;
            switch (i)
            {
                case 1: checkList = cellCol1; break;
                case 2: checkList = cellCol2; break;
                case 3: checkList = cellCol3; break;
                case 4: checkList = cellCol4; break;
                case 5: checkList = cellCol5; break;
                default: checkList = null; break;
            }
            if (checkList == null)
            {
                Debug.LogError("Something wrong here !!!");
                return;
            }
            SpawnNewCellInCollom(i, list, checkList.Count);
        }

        for (int i = 1; i <= MAX_COL; i++)
        {
            var cells = allCellInCollom[i];

            cells.Sort((a, b) => a.transform.localPosition.y.CompareTo(b.transform.localPosition.y));
            cells = allCellInCollom[i].Distinct().ToList();

            cells.RemoveAll(x => !x.gameObject.activeInHierarchy);
            ReassignGridPos(i, cells);
            cells.RemoveAll(x => x.gridPosition.x != i); ;
        }
        Drop(true);
    }

    public void RemoveAllLowestCells()
    {
        for (int i = 0; i < lowestCells.Count; i++)
        {
            if (lowestCells[i] == null) continue;
            lowestCells[i].gameObject.SetActive(false);
            var fx = PoolSystem.Instance.GetObject(removeCellFx, lowestCells[i].transform.position);
            fx.ChangeColor(lowestCells[i].spriteRenderer.color);
        }
        CheckToSpawnNewCell(lowestCells);
    }

    private void GetLowestCells()
    {
        lowestCells.Clear();
        foreach (var cell in allCell)
        {
            if (cell.Value == (BigInteger)Mathf.Pow(2, minIndex))
            {
                lowestCells.Add(cell);
            }
        }
        lowestCells.Add(null);
    }

    private bool HasLose()
    {
        return allCell.All(x => x.ConectableCount == 0);
    }

    private void CollectConectedCell(List<Cell> conectedCell)
    {
        cellCol1.Clear();
        cellCol2.Clear();
        cellCol3.Clear();
        cellCol4.Clear();
        cellCol5.Clear();
        foreach (var item in conectedCell)
        {
            if (item == null || item.Equals(conectedCell.Last())) continue;
            switch (item.gridPosition.x)
            {
                case 1: cellCol1.Add(item); break;
                case 2: cellCol2.Add(item); break;
                case 3: cellCol3.Add(item); break;
                case 4: cellCol4.Add(item); break;
                case 5: cellCol5.Add(item); break;
            }
        }
    }

    public void ReassignGridPos(int collom, List<Cell> cells)
    {
        var gridY = MAX_ROW;
        for (int j = 0; j < cells.Count; j++)
        {
            cells[j].gridPosition = new GridPosition(collom, gridY);
            gridY--;
        }
    }

    private void SpawnNewCellInCollom(int collom, List<Cell> cellsInCollom, int amount)
    {
        var spawnPos = new Vector2(girdPosToLocal[new GridPosition(collom, 1)].x, cellSpawnPos.transform.position.y);
        for (int j = 0; j < amount; j++)
        {
            if (cellsInCollom.Count >= MAX_ROW) continue;
            var newCell = SpawnCellNew(spawnPos);
            spawnPos.y++;
            if (!cellsInCollom.Contains(newCell)) cellsInCollom.Add(newCell);
        }
    }

    private List<Cell> RecollectActiveCell(int collom)
    {
        var list = allCellInCollom[collom];
        list.Clear();
        foreach (var item in allCell)
        {
            if (item.gridPosition.x == collom && item.gameObject.activeInHierarchy) list.Add(item);
        }

        return list;
    }

    public void Drop(bool showAd = false)
    {
        for (int i = 1; i <= MAX_COL; i++)
        {
            var list = allCellInCollom[i];
            foreach (var item in list)
            {
                item.transform.DOLocalMoveY(girdPosToLocal[item.gridPosition].y, CELL_DROP_TIME);
            }
        }
        LeanTween.delayedCall(CELL_DROP_TIME, () =>
        {
            OnDoneCellMove();
            if (!showAd) return;
            if (Time.time - AdManagerMax.Instance.lastInterTime >= FirebaseManager.Instance.remoteConfig.TimeBetweenAds)
            {
                AdManagerMax.Instance.ShowIntertistial(() => AdManagerMax.Instance.lastInterTime = Time.time);
                FirebaseManager.Instance.LogIntertisial("Gameplay");
            }
        });
    }

    public void OnDoneCellMove()
    {
        UpdateCell(true);
        if (HasLose())
        {
            GameFlow.Instance.ShowLosePopup();
            return;
        }
        GameSystem.SaveUserDataToLocal();
        GameFlow.Instance.gameState = GameState.Playing;
    }

    public void UpdateCell(bool saveData = false)
    {
        allCell = GetComponentsInChildren<Cell>();
        cellDic.Clear();
        foreach (var item in allCell)
        {
            cellDic.Add(item.gridPosition, item);
            if (!allCellInCollom.ContainsKey(item.gridPosition.x)) allCellInCollom.Add(item.gridPosition.x, new List<Cell>());
            var list = allCellInCollom[item.gridPosition.x];
            if (!list.Contains(item)) list.Add(item);
        }
        //debugCellCol1.Clear ();
        //debugCellCol2.Clear ();
        //debugCellCol3.Clear ();
        //debugCellCol4.Clear ();
        //debugCellCol5.Clear ();
        //debugCellCol1.AddRange (allCellInCollom[1]);
        //debugCellCol2.AddRange (allCellInCollom[2]);
        //debugCellCol3.AddRange (allCellInCollom[3]);
        //debugCellCol4.AddRange (allCellInCollom[4]);
        //debugCellCol5.AddRange (allCellInCollom[5]);
        var userCellDic = GameSystem.userdata.gameData.cellDic;
        foreach (var item in allCell)
        {
            item.FindNearbyCells();
            if (saveData) userCellDic[item.gridPosition.ToString()] = item.Value;
        }
        if (saveData) GameSystem.SaveUserDataToLocal();
    }


    public void LoadCells()
    {
        var userCellDic = GameSystem.userdata.gameData.cellDic;
        if (userCellDic != null && userCellDic.Count > 0)
        {
            foreach (var cell in allCell)
            {
                cell.Value = userCellDic[cell.gridPosition.ToString()];
            }
            return;
        }
        int posRandStart = Random.Range(0, allCell.Length);
        for (int i = 0; i < allCell.Length; i++)
        {
            if (indexStart == 0 || i != posRandStart) allCell[i].Value = ValueRandom();
            else
            {
                allCell[i].Value = (BigInteger)Mathf.Pow(2, indexStart);
                indexStart = 0;
            }
            userCellDic.Add(allCell[i].gridPosition.ToString(), allCell[i].Value);
        }
        GameSystem.SaveUserDataToLocal();
    }

    public void DoubleHightCellValue()
    {
        GameFlow.Instance.gameState = GameState.Fx;
        BigInteger highest;
        Cell cell = null;
        foreach (var item in allCell)
        {
            if (item.Value > highest)
            {
                cell = item;
                highest = item.Value;
            }
        }
        var cellScale = cell.transform.localScale;
        var sq = DOTween.Sequence();
        sq.AppendCallback(() => cell.transform.DOScale(1.2f, Const.DEFAULT_TWEEN_TIME));
        sq.AppendInterval(Const.DEFAULT_TWEEN_TIME);
        sq.AppendCallback(() =>
        {
            LeanTween.value((float)cell.Value, (float)cell.Value * 2, Const.DEFAULT_TWEEN_TIME).setAlpha().setOnUpdate(x =>
            {
                cell.valueTxt.text = ((BigInteger)x).ToString();
            });
            cell.spriteRenderer.DOColor(CellColor.Instance.GetCellColor(cell.Value * 2), Const.DEFAULT_TWEEN_TIME);
        });
        sq.AppendInterval(Const.DEFAULT_TWEEN_TIME);
        sq.AppendCallback(() =>
        {
            cell.Value *= 2;
            cell.transform.DOScale(cellScale, Const.DEFAULT_TWEEN_TIME);
        });
        sq.AppendInterval(Const.DEFAULT_TWEEN_TIME);
        sq.AppendCallback(() =>
        {
            var userData = GameSystem.userdata;
            userData.highestCellValue = cell.Value;
            GameSystem.SaveUserDataToLocal();
            GameFlow.Instance.gameState = GameState.Playing;
        });
    }

    [ContextMenu("Spawn")]
    public void SpawnGrid()
    {
        var pos = new Vector2((int)(-MAX_COL / 2f), (MAX_ROW / 2f) - 0.5f);
        var gridPos = new GridPosition(1, 1);
        for (int x = 0; x < MAX_COL; x++)
        {
            for (int y = 0; y < MAX_ROW; y++)
            {
                var cell = Instantiate(cellPrefab, pos, Quaternion.identity);
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
}
