using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public class Cell : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler,IPointerEnterHandler
{
    private Camera mainCam;
    void Start()
    {
        mainCam = Camera.main;
    }

    public void OnPointerEnter (PointerEventData eventData)
    {

    }

    public void OnBeginDrag (PointerEventData eventData)
    {
        Debug.Log ("Begin");
        Line.Instance.AddSegment (transform.position);
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
        //line.positionCount = 0;
    }
}
