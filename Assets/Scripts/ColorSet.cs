using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Cell))]
public class ColorSet : MonoBehaviour
{
    [SerializeField] private Color[] colors;
    private SpriteRenderer spriteRenderer;
    private Cell cell;

    private void Awake ()
    {
        cell = GetComponent<Cell>();
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    private void Start ()
    {
        var pow = (int)MathF.Log (cell.Value, 2);
        var index = pow % colors.Length;
        spriteRenderer.color = colors[index];
    }
}
