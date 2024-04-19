using System;
using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using UnityEngine;

[RequireComponent(typeof(Cell))]
public class ColorSet : MonoBehaviour
{
    [SerializeField] private Color[] colors;
    [SerializeField] private SpriteRenderer spriteRenderer;
    [SerializeField] private Cell cell;

    private void Awake ()
    {
        cell = GetComponent<Cell>();
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    public void SetColor ()
    {
        Mathf mathf;
        int pow;
        if (cell.Value <2)
        {
            pow = mathf.LogBigInt(GameFlow.Instance.TotalPoint, 2);
            Debug.Log(cell.Value + " " + GameFlow.Instance.TotalPoint);
        }
        else
        {
            pow = mathf.LogBigInt(cell.Value, 2);
        }
        var index = pow % colors.Length;
        var color = colors[index];
        color.a = 1f;
        spriteRenderer.color = color;
    }

    public Color GetColor(BigInteger value)
    {
        Mathf mathf;
        var pow = mathf.LogBigInt(cell.Value, 2);
        var index = pow % colors.Length;
        var color = colors[index];
        color.a = 1f;
        return color;
    }
}
