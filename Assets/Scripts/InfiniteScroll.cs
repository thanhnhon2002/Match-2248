using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class InfiniteScroll : MonoBehaviour,IEndDragHandler, IDragHandler, IBeginDragHandler, IPointerDownHandler
{
    [SerializeField] private Road[] roads;
    private Vector2 lastPos;
    private Vector2 beginPos;
    private float delta;
    private void Awake ()
    {
        roads = GetComponentsInChildren<Road>();
    }

    public void OnPointerDown (PointerEventData eventData)
    {
        for (int i = 0; i < roads.Length; i++)
        {
            roads[i].Move (0f, Vector2.zero);
        }
    }

    public void OnBeginDrag (PointerEventData eventData)
    {
        beginPos = eventData.position;   
        lastPos = eventData.position;   
    }

    public void OnDrag (PointerEventData eventData)
    {
        delta = eventData.position.y - lastPos.y;
        for (int i = 0; i < roads.Length; i++)
        {
            var pos = roads[i].rectTransform.anchoredPosition;
            pos = new Vector2 (0, pos.y + delta);
            roads[i].rectTransform.anchoredPosition = pos;
        }
        lastPos = eventData.position;
    }
    public void OnEndDrag (PointerEventData eventData)
    {
        var direction = eventData.position - beginPos;
        for (int i = 0; i < roads.Length; i++)
        {
            roads[i].Move (direction.magnitude, direction.normalized);
        }
    }
}
