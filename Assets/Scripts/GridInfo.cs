using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Numerics;
using System.Runtime.CompilerServices;
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

    [MethodImpl (MethodImplOptions.AggressiveInlining)]
    public override string ToString ()
    {
        return ToString (null, null);
    }

    [MethodImpl (MethodImplOptions.AggressiveInlining)]
    public string ToString (string format)
    {
        return ToString (format, null);
    }

    [MethodImpl (MethodImplOptions.AggressiveInlining)]
    public string ToString (string format, IFormatProvider formatProvider)
    {
        if (string.IsNullOrEmpty (format))
        {
            format = "F2";
        }

        if (formatProvider == null)
        {
            formatProvider = CultureInfo.InvariantCulture.NumberFormat;
        }

        return string.Format ("({0}, {1})", x.ToString (format, formatProvider), y.ToString (format, formatProvider));
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
        //cell.Value = (BigInteger)Mathf.Pow (2, Random.Range (1, 5));
        cell.gridPosition = position;
        //manager.allCell.Add (cell);
    }
}
