using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Player : MonoBehaviour
{
    public static Player Instance { get; private set; }
    [SerializeField] private List<Vector3> segments = new List<Vector3> ();
    private LineRenderer lineRenderer;
    private Camera mainCam;
    private bool init;
    private int segmentCount;
    private int initValue;
    private List<int> conectedValue = new List<int> ();

    private void Awake ()
    {
        Instance = this;
        lineRenderer = GetComponent<LineRenderer> ();
        mainCam = Camera.main;
    }

    private void Update ()
    {
        if (segments.Count > 1) segments[segments.Count - 1] = mainCam.ScreenToWorldPoint (Input.mousePosition);
        lineRenderer.SetPositions (segments.ToArray ());
    }

    public void InitLine (Cell cell)
    {
        if (segments.Contains (cell.transform.position)) return;
        initValue = cell.Value;
        conectedValue.Add(cell.Value);
        segmentCount++;
        GameFlow.Instance.TotalPoint = initValue;
        segments.Add (cell.transform.position);
        segments.Add (Vector3.zero);
        lineRenderer.positionCount = segments.Count;
        //lineRenderer.colorGradient.SetKeys (new GradientColorKey(Color.green,);
        init = true;
    }

    public void AddSegment(Cell cell)
    {
        if (!init) return;
        if (segments.Contains (cell.transform.position)) return;
        if(CanConect(cell))
        {
            segments[segments.Count - 1] = cell.transform.position;
            segments.Add (Vector3.zero);
            lineRenderer.positionCount = segments.Count;
            segmentCount += cell.Value / initValue;
            GameFlow.Instance.CalculateTotal (initValue, segmentCount);
        }       
    }

    private bool CanConect(Cell cell)
    {
        if (conectedValue.Contains (cell.Value) && initValue == cell.Value) return true;
        //var currentValue = GameFlow.Instance.TotalPoint;
        if (cell.Value > initValue && conectedValue.Contains (cell.Value / 2))
        {
            initValue = cell.Value;
            conectedValue.Add (cell.Value);
            return true;
        }
        return false;
    }

    public void ClearLine ()
    {
        conectedValue.Clear ();
        segments.Clear ();
        segmentCount = 0;
        lineRenderer.positionCount = 0;
        init = false;
    }
}
