using System;
using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using UnityEngine;

//[RequireComponent(typeof(Cell))]
public class ColorSet : MonoBehaviour
{
    [SerializeField] private SpriteRenderer spriteRenderer;
    [SerializeField] private Cell cell;
    [SerializeField] private CellTutorial cellTt;
    public bool isTutorial;

    private void Awake ()
    {
        if(!isTutorial) cell = GetComponent<Cell>();
        else cellTt = GetComponent<CellTutorial>();
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    public void SetColor ()
    {
        Color color;
        if (!isTutorial)
        {
             color = CellColor.Instance.GetCellColor(cell.Value);
        }
        else
        {
            color = CellColor.Instance.GetCellColor(cellTt.Value);
        }
        color.a = 1f;
        spriteRenderer.color = color;
    }
}
