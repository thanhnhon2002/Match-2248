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
    public const bool SHOW_DEBUG_TEXT = false;

    public SpriteRenderer spriteRenderer { get; private set; }
    public GameObject highLight;
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
            valueTxt.text = BigIntegerConverter.ConvertNameValue (value);
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
        highLight = transform.Find("HighLight").gameObject;
        debugTxt.text = "";
#if !UNITY_EDITOR
        debugTxt.gameObject.SetActive (false);
#endif
    }

#if UNITY_EDITOR
    private void Update ()
    {
        if (SHOW_DEBUG_TEXT)
        {
            debugTxt.text = $"({gridPosition.x},{gridPosition.y})";
        }
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
        Hint.Instance.StartHint();
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
        Hint.Instance.ClearList();
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
        Hint.Instance.StartHint();
        switch (GameFlow.Instance.gameState)
        {
            case GameState.Playing:
                Player.Instance.ClearLine();               
                Player.Instance.EndDrag();               
                break;
            case GameState.Paint:
                Paint.Instance.PaintAllCells();
                break;
            default: return;
        }
    }

    public void IncreaseValue(BigInteger endValue)
    {
        spriteRenderer.DOColor(CellColor.Instance.GetCellColor(endValue), Const.DEFAULT_TWEEN_TIME);
        LeanTween.value(1000, 9999, Const.DEFAULT_TWEEN_TIME).setOnUpdate((float x) =>
        {
            valueTxt.text = x.ToString().Substring(0, 4);
        });
    }

#if UNITY_EDITOR
    private void OnValidate ()
    {
        valueTxt.text = BigIntegerConverter.ConvertNameValue (Value);
    }
#endif
}
