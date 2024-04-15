using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Cell))]
public class SpriteSet : MonoBehaviour
{
    [SerializeField] private Sprite[] sprites;
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
        var index = pow % sprites.Length;
        spriteRenderer.sprite = sprites[index];
    }
}
