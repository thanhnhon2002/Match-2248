using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using System.Linq;
using System.Numerics;

public class Cell : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler, IPointerEnterHandler
{
    public SpriteRenderer spriteRenderer { get; private set; }
    [SerializeField] private TextMeshPro valueTxt;
    public ColorSet colorSet { get; private set; }
    public List<Cell> nearbyCell = new List<Cell>();
    public GridPosition gridPosition;
    public UnityEvent OnInteract;
    private BigInteger value;
    public BigInteger Value
    {
        get { return value; }
        set {
            this.value = value;
            valueTxt.text = BigIntegerConverter.ConverNameValue(value);
            colorSet.SetColor();
        }
    }


    private void Awake ()
    {
        spriteRenderer = GetComponent<SpriteRenderer> ();
        colorSet = GetComponent<ColorSet> ();
    }

    public void FindNearbyCells()
    {
        nearbyCell.Clear ();
        var listDirection = GridManager.neighbourGridPosition;

        for (int i = 0; i < listDirection.Count; i++)
        {
            var dir = listDirection[i];
            var nearbyX = gridPosition.x + dir.x;
            if (nearbyX < 1) nearbyX = 1;
            var nearbyY = gridPosition.y + dir.y;
            if(nearbyY < 1) nearbyY = 1;
            var cell = GridManager.Instance.GetCellAt (new GridPosition (nearbyX, nearbyY));
            if(cell != null) nearbyCell.Add (cell);
        }
        nearbyCell = nearbyCell.Distinct ().ToList();
        if(nearbyCell.Contains(this)) nearbyCell.Remove (this);
    }

    public void OnPointerEnter (PointerEventData eventData)
    {
        Player.Instance.CheckCell (this);
    }

    public void OnBeginDrag (PointerEventData eventData)
    {
        Player.Instance.InitLine (this);
    }

    public void OnDrag (PointerEventData eventData)
    {
        //Do not delete this funtion or esle it won't work. I don't know why
    }

    public void OnEndDrag (PointerEventData eventData)
    {
        Player.Instance.ClearLine();
    }

    private void OnValidate ()
    {
        valueTxt.text = BigIntegerConverter.ConverNameValue(Value);
    }

    //[ContextMenu("Highligth")]
    //public void Highight()
    //{
    //    foreach (var item in nearbyCell)
    //    {
    //        item.spriteRenderer.color = Color.red;
    //    }
    //}
}
