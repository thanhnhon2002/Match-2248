using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEditor;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public class Cell : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler, IPointerEnterHandler
{
    public SpriteRenderer spriteRenderer { get; private set; }
    [SerializeField] private TextMeshPro valueTxt;
    public List<Cell> nearbyCell = new List<Cell>();
    public GridPosition gridPosition;
    public UnityEvent OnInteract;
    private int value;
    public int Value
    {
        get { return value; }
        set {
            this.value = value;
            valueTxt.text = value.ToString ();
        }
    }


    private void Awake ()
    {
        spriteRenderer = GetComponent<SpriteRenderer> ();
    }

    private void Start ()
    {
        FindNearbyCells ();
    }

    private void FindNearbyCells()
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
        Player.Instance.ClearLine ();
    }

    private void OnValidate ()
    {
        valueTxt.text = value.ToString ();
    }
}
