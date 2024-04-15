using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

public class Cell : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler,IPointerEnterHandler
{
    [SerializeField ] private TextMeshPro valueTxt;
    [SerializeField] private int value;
    public int Value
    {
        get { return value; }
        set 
        {
            this.value = value; 
            valueTxt.text = value.ToString();
        }
    }

    public void OnPointerEnter (PointerEventData eventData)
    {
        Player.Instance.AddSegment (this);
    }

    public void OnBeginDrag (PointerEventData eventData)
    {
        Debug.Log ("Begin");
        Player.Instance.InitLine (this);
    }

    public void OnDrag (PointerEventData eventData)
    {
        Debug.Log ("Drag");
        //var pos = mainCam.ScreenToWorldPoint (eventData.position);
        //line.SetPosition (1, pos);
    }

    public void OnEndDrag (PointerEventData eventData)
    {
        Debug.Log ("End");
        Player.Instance.ClearLine ();
    }

    private void OnValidate ()
    {
        valueTxt.text = value.ToString ();
    }
}
