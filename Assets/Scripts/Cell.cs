using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using System.Linq;
using System.Numerics;
using DG.Tweening;

public class Cell : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler, IPointerEnterHandler, IPointerExitHandler
{
    public SpriteRenderer spriteRenderer { get; private set; }
    public TextMeshPro valueTxt;
    public ColorSet colorSet { get; private set; }
    public List<Cell> nearbyCell = new List<Cell>();
    public GridPosition gridPosition;
    public UnityEvent OnInteract;
    private BigInteger value;
    public int indexDrop;
    public BigInteger Value
    {
        get { return value; }
        set {
            this.value = value;
            valueTxt.text = BigIntegerConverter.ConverNameValue(value);
            colorSet.SetColor();
        }
    }
    public void Drop()
    {
        if (indexDrop > 0) transform.DOMoveY(transform.position.y - 1, 0.2f).SetEase(Ease.Linear).OnComplete(()=>
        {
            indexDrop--;
            Drop();
        });
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
        //for (int i = 0;i < nearbyCell.Count;i++)
        //{
        //    nearbyCell[i].spriteRenderer.color = Color.red;
        //}
        Player.Instance.CheckCell (this);
    }
    public void OnPointerExit (PointerEventData eventData)
    {
        for (int i = 0; i < nearbyCell.Count; i++)
        {
            nearbyCell[i].colorSet.SetColor ();
        }
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
