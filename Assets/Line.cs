using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Line : MonoBehaviour
{
    public static Line Instance { get; private set; }
    private List<Vector3> segments = new List<Vector3>();
    private LineRenderer lineRenderer;

    private void Awake ()
    {
        Instance = this;
        lineRenderer = GetComponent<LineRenderer>();
    }

    private void Update ()
    {
        lineRenderer.SetPositions(segments.ToArray());
    }

    public void AddSegment(Vector3 segment)
    {
        if(segments.Contains(segment)) return;  
        segments.Add(segment);
    }

    public void ClearLine()
    {
        for (int i = 0; i < segments.Count; i++)
        {
            segments[i] = Vector3.zero;
        }
    }
}
