using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class Effect : MonoBehaviour
{
    [SerializeField] private CellPartical[] cellPartcals;
    [SerializeField] private float time;
    [SerializeField] private List<Vector3> pos = new List<Vector3>();
    private Sequence sq;
    public void Play (List<Cell> path, int index)
    {
        pos.Clear ();
        for (int i = index; i < path.Count; i++)
        {
            pos.Add (path[i].transform.position);
        }
        if (sq != null) sq.Play ();
        else
        {
            sq = DOTween.Sequence ();
            sq.AppendCallback (() =>
            {
                for (int i = 0; i < cellPartcals.Length; i++)
                {
                    cellPartcals[i].PlayEffectOut (time);
                }
            });
            sq.AppendInterval (time);
            sq.AppendCallback (() =>
            {
                for (int i = 0; i < cellPartcals.Length; i++)
                {
                    cellPartcals[i].PlayEffectIn (time);
                }
                transform.DOPath (pos.ToArray (), time);
            });
        }
    }

    [ContextMenu("Set up")]
    private void SetUp()
    {
        for (int i = 0; i < transform.childCount; i++)
        {
            var child = transform.GetChild (i);
            var pos = Random.insideUnitCircle * 1.5f;
            var scale = Random.Range (0.3f, 1f);
            var rotation = Random.Range (0f, 360f);
            child.localPosition = pos;
            child.transform.localScale = Vector3.one * scale;
            child.transform.Rotate (0f, 0f, rotation);
        }
    }
}
