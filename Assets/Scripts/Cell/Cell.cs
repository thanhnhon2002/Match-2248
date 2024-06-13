using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using System.Linq;
using System.Numerics;

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
        if (GameFlow.Instance.gameState != GameState.Playing) return;
        Player.Instance.CheckCell (this);
    }

    public void OnBeginDrag (PointerEventData eventData)
    {
        if (GameFlow.Instance.gameState != GameState.Playing) return;
        Player.Instance.InitLine (this);
    }

    public void OnDrag (PointerEventData eventData)
    {
        //Do not delete this funtion or esle it won't work. I don't know why
    }

    public void OnEndDrag (PointerEventData eventData)
    {
        if (GameFlow.Instance.gameState != GameState.Playing) return;
        Player.Instance.ClearLine ();
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
