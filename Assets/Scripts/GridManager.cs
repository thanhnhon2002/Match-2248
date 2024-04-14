using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UnityEngine.UIElements;

public class GridManager : MonoBehaviour
{
    public readonly int MAX_ROW = 8;
    public readonly int MAX_COL = 5;

    [SerializeField] private GridInfo girdPrefab;
    public GridInfo[] allGrid { get; private set; }

    private Dictionary<int, List<GridInfo>> rows = new Dictionary<int, List<GridInfo>>();
    private Dictionary<int, List<GridInfo>> cols = new Dictionary<int, List<GridInfo>>();

    public Dictionary<int, List<GridInfo>> Rows => rows;
    public Dictionary<int, List<GridInfo>> Cols => cols;

    [SerializeField] private Vector2 maxPos;
    [SerializeField] private Vector2 minPos;

    [ContextMenu("Init")]
    private void InitData()
    {
        allGrid = GetComponentsInChildren<GridInfo>();
        foreach (var info in allGrid)
        {
            //info.GetNearbyGrid();
            if (!rows.ContainsKey(info.position.y)) rows.Add(info.position.y, new List<GridInfo>());
            rows[info.position.y].Add(info);
            if (!cols.ContainsKey(info.position.x)) cols.Add(info.position.x, new List<GridInfo>());
            cols[info.position.x].Add(info);
        }
    }
    public GridInfo GetGridAt(int x, int y)
    {
        var grid = allGrid.FirstOrDefault(g => g.position.x == x && g.position.y == y);
        return grid;
    }

    public GridInfo GetGridAt (GridPosition position)
    {
        var grid = allGrid.FirstOrDefault (g => g.position.x == position.x && g.position.y == position.y);
        return grid;
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
                var grid = Instantiate (girdPrefab, pos, Quaternion.identity, transform);
                grid.position = gridPos;
                gridPos.y += 1;
                pos.y -= 1f;
#if UNITY_EDITOR
                var randomSprite = grid.GetComponent<RandomSprites> ();
                if (randomSprite) randomSprite.Awake ();
#endif
            }
            gridPos.x += 1;
            gridPos.y = 1;
            pos.x += 1f;
            pos.y = (MAX_ROW / 2f) - 0.5f;
        }
        InitData();
    }
}
