using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;


[System.Serializable]
public struct GridPosition
{
    public int x;
    public int y;

    public GridPosition (int x, int y)
    {
        this.x = x;
        this.y = y;
    }
}
public class GridInfo : MonoBehaviour
{
    public GridManager manager;
    public GridPosition position;
    [SerializeField] private Cell cellPrefab;
    private List<GridPosition> neighbourGridPosition = new List<GridPosition> ()
    { new GridPosition(0, 1), new GridPosition (1,1), new GridPosition(1, 0), new GridPosition(1,-1), new GridPosition(0, -1), new GridPosition(-1, -1), new GridPosition(-1, 0), new GridPosition(-1,1) };

    private void Awake ()
    {
        var cell = PoolSystem.Instance.GetObject (cellPrefab, transform.position);
        cell.Value = (int)Mathf.Pow (2, Random.Range (1, 5));
    }
}
