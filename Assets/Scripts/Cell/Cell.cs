using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using System.Linq;
using System.Numerics;
using DG.Tweening;
public class Cell : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler, IPointerEnterHandler, IPointerClickHandler
{
    public SpriteRenderer spriteRenderer { get; private set; }
    public TextMeshPro valueTxt;
    public ColorSet colorSet { get; private set; }
    public HighCellEffect highCellEffect { get; private set; }
    public List<Cell> nearbyCell = new List<Cell> ();
    public GridPosition gridPosition;
    public UnityEvent OnInteract;
    private BigInteger value;
    public BigInteger Value
    {
        get { return value; }
        set {
            this.value = value;
            valueTxt.text = BigIntegerConverter.ConverNameValue (value);
            colorSet.SetColor ();
        }
    }
    public int ConectableCount
    {
        get {
            return nearbyCell.Count (x => x.Value == value);
        }
    }

    public TextMeshPro debugTxt;

    private void Awake ()
    {
        spriteRenderer = GetComponent<SpriteRenderer> ();
        colorSet = GetComponent<ColorSet> ();
        highCellEffect = GetComponent<HighCellEffect> ();
#if !UNITY_EDITOR
        debugTxt.gameObject.SetActive (false);
#endif
    }

#if UNITY_EDITOR
    private void Update ()
    {
        debugTxt.text = $"x = {gridPosition.x}, y =  {gridPosition.y}";
    }
#endif
    public void FindNearbyCells ()
    {
        nearbyCell.Clear ();
        var listDirection = GridManager.neighbourGridPosition;

        for (int i = 0; i < listDirection.Count; i++)
        {
            var dir = listDirection[i];
            var nearbyX = gridPosition.x + dir.x;
            if (nearbyX < 1) nearbyX = 1;
            var nearbyY = gridPosition.y + dir.y;
            if (nearbyY < 1) nearbyY = 1;
            var cell = GridManager.Instance.GetCellAt (new GridPosition (nearbyX, nearbyY));
            if (cell != null) nearbyCell.Add (cell);
        }
        nearbyCell = nearbyCell.Distinct ().ToList ();
        if (nearbyCell.Contains (this)) nearbyCell.Remove (this);
    }

    public void OnPointerClick (PointerEventData eventData)
    {
        switch (GameFlow.Instance.gameState)
        {
            case GameState.Swap:
                Swap.Instance.ChoseCell (this);
                break;
            case GameState.Smash:
                Hammer.Instance.Smash (this);
                break;
            default: return;
        }
    }

    public void OnPointerEnter (PointerEventData eventData)
    {
        switch(GameFlow.Instance.gameState)
        {
            case GameState.Playing:
                Player.Instance.CheckCell(this);
                break;
            case GameState.Paint:
                Paint.Instance.CheckCell(this);
                break;
            default: return;
        }     
    }

    public void OnBeginDrag (PointerEventData eventData)
    {
        switch (GameFlow.Instance.gameState)
        {
            case GameState.Playing:
                Player.Instance.InitLine(this);
                break;
            case GameState.Paint:
                Paint.Instance.BeginPaint(this);
                break;
            default: return;
        }
    }

    public void OnDrag (PointerEventData eventData)
    {
        //Do not delete this funtion or esle it won't work. I don't know why
    }

    public void OnEndDrag (PointerEventData eventData)
    {
        switch (GameFlow.Instance.gameState)
        {
            case GameState.Playing:
                Player.Instance.ClearLine();
                break;
            case GameState.Paint:
                Paint.Instance.PaintAllCells();
                break;
            default: return;
        }
        if (GameFlow.Instance.gameState != GameState.Playing) return;
        
    }

    public void IncreaseValue(BigInteger endValue)
    {
        spriteRenderer.DOColor(CellColor.Instance.GetCellColor(endValue), Const.DEFAULT_TWEEN_TIME);
        LeanTween.value((float)Value, (float)endValue, Const.DEFAULT_TWEEN_TIME).setOnUpdate((float x) =>
        {
            valueTxt.text = x.ToString().Substring(0,2);
        }).setOnComplete(() => Value = endValue);
    }

#if UNITY_EDITOR
    private void OnValidate ()
    {
        valueTxt.text = BigIntegerConverter.ConverNameValue (Value);
    }
#endif
    //[ContextMenu("Highligth")]
    //public void Highight()
    //{
    //    foreach (var item in nearbyCell)
    //    {
    //        item.spriteRenderer.color = Color.red;
    //    }
    //}
}
