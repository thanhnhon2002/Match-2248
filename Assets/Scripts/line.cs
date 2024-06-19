using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Line : MonoBehaviour
{
    public LineRenderer lineRenderer { get; private set; }
    [SerializeField] private int sortingOrder = -1;
    public Cell startCell;
    public Cell endCell;

    private void Awake ()
    {
        lineRenderer = GetComponent<LineRenderer> ();   
    }

    private void OnEnable ()
    {
        lineRenderer.sortingOrder = sortingOrder;
    }

    private void Update ()
    {
        if (endCell == null) lineRenderer.SetPosition (1, Player.Instance.mousePos);
    }

    public void SetLine(Cell start, Cell end)
    {
        startCell = start;
        endCell = end;
        lineRenderer.SetPosition (0, start.transform.position);
        if (endCell == null) lineRenderer.SetPosition (1, Player.Instance.mousePos);
        else lineRenderer.SetPosition (1, end.transform.position);
    }

    public void SetColor(Color color)
    {
        lineRenderer.startColor = color;
        lineRenderer.endColor = color;  
    }
}
