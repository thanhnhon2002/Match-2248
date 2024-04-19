using System;
using System.Collections;
using System.Collections.Generic;
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
        var pow = (int)MathF.Log (cell.Value, 2);
        var index = pow % colors.Length;
        var color = colors[index];
        color.a = 1f;
        spriteRenderer.color = color;
    }

    public Color GetColor(int value)
    {
        var pow = (int)MathF.Log (value, 2);
        var index = pow % colors.Length;
        var color = colors[index];
        color.a = 1f;
        return color;
    }
}
