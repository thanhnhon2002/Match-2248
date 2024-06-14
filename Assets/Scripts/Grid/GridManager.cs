using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using DG.Tweening;
using System.Numerics;
using Random = UnityEngine.Random;
using Vector2 = UnityEngine.Vector2;
using Vector3 = UnityEngine.Vector3;
using Quaternion = UnityEngine.Quaternion;
using DarkcupGames;
using DeepTrackSDK;
using Castle.Core.Internal;
using System;

public class GridManager : MonoBehaviour
{
    private const float CELL_DROP_TIME = 0.5F;
    public const int MIN_HIGHLIGHT_VALUE = 128;
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
    public Dictionary<GridPosition, Vector3> girdPosToLocal { get; private set; } = new Dictionary<GridPosition, Vector3>();
    public Cell[] allCell { get; private set; }
    public Dictionary<GridPosition, Cell> cellDic = new Dictionary<GridPosition, Cell>();
    public OnSceneChangeCellEffect effect { get; private set; }

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


    private void Awake()
    {
        DOTween.SetTweensCapacity(1500, 1500);
        Application.targetFrameRate = 60;
        Instance = this;
        effect = GetComponent<OnSceneChangeCellEffect>();
    }

    private void Start()
    {
        //Debug.LogError(BigIntegerConverter.ConverNameValue(BigInteger.Parse("4000000000000")));
        //Debug.LogError(BigIntegerConverter.ConverNameValue(BigInteger.Parse("8000000000000000")));
        //Debug.LogError(BigIntegerConverter.ConverNameValue(BigInteger.Parse("1600000000000000000")));
        //Debug.LogError(BigIntegerConverter.ConverNameValue(BigInteger.Parse("3200000000000000000000")));
        //Debug.LogError(BigIntegerConverter.ConverNameValue(BigInteger.Parse("6400000000000000000000000")));
        //Debug.LogError(BigIntegerConverter.ConverNameValue(BigInteger.Parse("1280000000000000000000000000")));
        //Debug.LogError(BigIntegerConverter.ConverNameValue(BigInteger.Parse("2560000000000000000000000000000")));
        //Debug.LogError(BigIntegerConverter.ConverNameValue(BigInteger.Parse("2560000000000000000000000000000000000000000000000000000000000000000000000000000000000")));
        //Debug.LogError(BigIntegerConverter.ConverNameValue(BigInteger.Parse("2560000000000000000000000000000000000000000000000000000000000000000000000000000000000000")));
        //Debug.LogError(BigIntegerConverter.ConverNameValue(BigInteger.Parse("2560000000000000000000000000000000000000000000000000000000000000000000000000000000000000000")));
        //Debug.LogError(BigIntegerConverter.ConverNameValue(BigInteger.Parse("2560000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000")));
        //Debug.LogError(BigIntegerConverter.ConverNameValue(BigInteger.Parse("2560000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000")));
        //Debug.LogError(BigIntegerConverter.ConverNameValue(BigInteger.Parse("2560000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000")));
        //Debug.LogError(BigIntegerConverter.ConverNameValue(BigInteger.Parse("25600000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000002560000000000000000000000000000000000000000000000000000000000000000000000000000000000000000")));
        //Debug.LogError(BigIntegerConverter.ConverNameValue(BigInteger.Parse("2560000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000256000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000")));
        ////Debug.LogError(BigIntegerConverter.ConverNameValue(BigInteger.Parse("25600000000000000000000000000000000000000000000000000000000000000000000000000000000000000000002560000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000256000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000025600000000000000000000000000000000000000000000000000000000000000000000000000000000000000000002560000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000256000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000025600000000000000000000000000000000000000000000000000000000000000000000000000000000000000000002560000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000256000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000025600000000000000000")));
        allCell = GetComponentsInChildren<Cell>();
        var userData = GameSystem.userdata;
        if (userData.replay)
        {
            foreach (var item in allCell)
            {
                item.gameObject.SetActive(false);
            }
            //StartCoroutine(WaitChoseStart());
        }
        else
        {
            SetUpCell();
            HighlightHighestCell();
        }
        FirebaseManager.Instance.SetProperty(UserPopertyKey.last_level, maxIndex.ToString());
    }
    private void SetUpCell()
    {
        var userData = GameSystem.userdata;
        if (userData.gameData.minIndex == 0)
        {
            minIndex = 1;
            maxIndex = Space_Index + minIndex - 1;
            maxIndexRandom = (int)(maxIndex + minIndex) / 2 + 2;
            indexPlayer = maxIndexRandom;
            FirebaseManager.Instance.LogLevelStart(maxIndexRandom, false);
            DeepTrack.LogLevelStart(maxIndexRandom);
        }
        else LoadDataIndex();
        foreach (var item in allCell)
        {
            girdPosToLocal.Add(item.gridPosition, item.transform.localPosition);
        }
        UpdateCell();
        LoadCells();
    }

    public void SetStartIndex()
    {
        foreach (var item in allCell)
        {
            item.gameObject.SetActive(true);
        }
        if(indexChose <= 0) indexChose = 1;
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
        FirebaseManager.Instance.LogLevelStart(maxIndexRandom, true);
        DeepTrack.LogLevelStart(maxIndexRandom);
    }

    public Cell SpawnCell(Vector2 position, int value)
    {
        var cell = PoolSystem.Instance.GetObject(cellPrefab, position);
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
            DeepTrack.LogLevelWin(index);
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
            List<Cell> checkList = i switch
            {
                1 => cellCol1,
                2 => cellCol2,
                3 => cellCol3,
                4 => cellCol4,
                5 => cellCol5,
                _ => null
            }; 
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
            AdManagerMax.Instance.ShowIntertistial("Gameplay", null);          
        });
    }

    public void OnDoneCellMove()
    {
        UpdateCell(true);
        if (HasLose())
        {
            GameFlow.Instance.ShowLosePopup();
            DeepTrack.LogLevelLose(GameSystem.userdata.level);
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
        var userCellDic = GameSystem.userdata.gameData.cellDic;
        Mathf math;
        foreach (var item in allCell)
        {
            item.FindNearbyCells();
            if (saveData) userCellDic[item.gridPosition.ToString()] = math.LogBigInt(item.Value, 2);
        }
        HighlightHighestCell();
        if (saveData) GameSystem.SaveUserDataToLocal();
    }

    public void HighlightHighestCell()
    {
        foreach (var item in allCell)
        {
            if (item.Value == GameSystem.userdata.gameData.currentHighestCellValue && item.Value > MIN_HIGHLIGHT_VALUE) item.highCellEffect.ShowEffect();
            else item.highCellEffect.StopEffect();
        }
    }

    private void LoadCells()
    {
        var userCellDic = GameSystem.userdata.gameData.cellDic;
        if (userCellDic != null && userCellDic.Count > 0)
        {
            foreach (var cell in allCell)
            {
                cell.Value = BigIntegerConverter.PowBigInterger(2, userCellDic[cell.gridPosition.ToString()]);
            }
            return;
        }
        Mathf math;
        int posRandStart = Random.Range(0, allCell.Length);
        for (int i = 0; i < allCell.Length; i++)
        {
            if (indexStart == 0 || i != posRandStart) allCell[i].Value = ValueRandom();
            else
            {
                allCell[i].Value = (BigInteger)Mathf.Pow(2, indexStart);
                //indexStart = 0;
            }
            userCellDic.Add(allCell[i].gridPosition.ToString(), math.LogBigInt(allCell[i].Value,2));
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
        cell.spriteRenderer.maskInteraction = SpriteMaskInteraction.None;
        var cellScale = cell.transform.localScale;
        Mathf math;
        var sq = DOTween.Sequence();
        sq.AppendCallback(() => cell.transform.DOScale(1.2f, Const.DEFAULT_TWEEN_TIME));
        sq.AppendInterval(Const.DEFAULT_TWEEN_TIME);
        sq.AppendCallback(() => cell.IncreaseValue(cell.Value * 2));
        sq.AppendInterval(Const.DEFAULT_TWEEN_TIME);
        sq.AppendCallback(() =>
        {
            cell.Value *= 2;
            cell.transform.DOScale(cellScale, Const.DEFAULT_TWEEN_TIME);
        });
        sq.AppendInterval(Const.DEFAULT_TWEEN_TIME);
        sq.AppendCallback(() =>
        {
            cell.spriteRenderer.maskInteraction = SpriteMaskInteraction.VisibleInsideMask;
            var userData = GameSystem.userdata;
            userData.gameData.currentHighestCellValue = cell.Value;
            if(userData.gameData.currentHighestCellValue > userData.highestCellValue) userData.highestCellValue = cell.Value;
            userData.gameData.cellDic[cell.gridPosition.ToString()] = math.LogBigInt(cell.Value,2);
            GameSystem.SaveUserDataToLocal();
            GameFlow.Instance.gameState = GameState.Playing;
        });
    }

    public void SpawHighestCell()
    {
        GameFlow.Instance.gameState = GameState.Fx;
        var targetValue = GameSystem.userdata.gameData.currentHighestCellValue;
        var lowCells = allCell.FindAll(x => x.Value < targetValue).ToList() ;
        var cell = lowCells.RandomElement();
        cell.spriteRenderer.maskInteraction = SpriteMaskInteraction.None;
        cell.spriteRenderer.sortingOrder = 1;
        cell.valueTxt.sortingOrder = 1;
        var cellScale = cell.transform.localScale;
        Mathf math;
        var sq = DOTween.Sequence();
        sq.AppendCallback(() => cell.transform.DOScale(1.2f, Const.DEFAULT_TWEEN_TIME));
        sq.AppendInterval(Const.DEFAULT_TWEEN_TIME);
        sq.AppendCallback(() =>
        {
            cell.valueTxt.DOFade(0f, Const.DEFAULT_TWEEN_TIME);
            cell.spriteRenderer.DOColor(CellColor.Instance.GetCellColor(targetValue), Const.DEFAULT_TWEEN_TIME);
        });
        sq.AppendInterval(Const.DEFAULT_TWEEN_TIME);
        sq.AppendCallback(() =>
        {
            cell.valueTxt.DOFade(1f, Const.DEFAULT_TWEEN_TIME);
            cell.Value = targetValue;
            cell.transform.DOScale(cellScale, Const.DEFAULT_TWEEN_TIME);
        });
        sq.AppendInterval(Const.DEFAULT_TWEEN_TIME);
        sq.AppendCallback(() =>
        {
            cell.spriteRenderer.maskInteraction = SpriteMaskInteraction.VisibleInsideMask;
            cell.spriteRenderer.sortingOrder = 0;
            cell.valueTxt.sortingOrder = 0;
            cell.highCellEffect.ShowEffect();
            var userData = GameSystem.userdata;
            userData.gameData.currentHighestCellValue = cell.Value;
            if (userData.gameData.currentHighestCellValue > userData.highestCellValue) userData.highestCellValue = cell.Value;
            userData.gameData.cellDic[cell.gridPosition.ToString()] = math.LogBigInt(cell.Value,2);
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

        var cells = FindObjectsOfType<Cell>();
        foreach (var item in cells)
        {
            item.transform.SetParent(transform);
        }
    }
}
