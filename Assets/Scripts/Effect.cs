using DarkcupGames;
using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Effect : MonoBehaviour
{
    [SerializeField] private CellPartical[] cellPartcals;
    [SerializeField] private List<Vector3> pos = new List<Vector3>();
    public float delayEachBlock = 0.1f;
    private Sequence sq;
    public void Play(List<Cell> path, int index, Color startColor, Color endColor)
    {
        pos.Clear();
        for (int i = index; i < path.Count; i++)
        {
            pos.Add(path[i].transform.position);
        }
        Play(index, startColor, endColor);
    }

    public void Play(List<CellTutorial> path, int index, Color startColor, Color endColor)
    {
        pos.Clear();
        for (int i = index; i < path.Count; i++)
        {
            pos.Add(path[i].transform.position);
        }
        Play(index, startColor, endColor);
    }

    private void Play(int index, Color startColor, Color endColor)
    {
        float outTime = Player.MAX_EFFECT_TIME * 0.3f + (pos.Count - index + 0.5f) * delayEachBlock * 0.5f;
        float inTime = Player.MAX_EFFECT_TIME * 0.5f;

        if (outTime < 0.01f) outTime = 0.01f;

        Debug.Log($"index = {index}, pos.Count = {pos.Count}, outTIme = {outTime}, inTime = {inTime}, outTime + inTime = {outTime + inTime}");

        startColor.a = 0;
        for (int i = 0; i < cellPartcals.Length; i++)
        {
            startColor.a = 1f;
            cellPartcals[i].spriteRenderer.color = startColor;
        }
        sq = DOTween.Sequence();
        sq.AppendCallback(() =>
        {
            for (int i = 0; i < cellPartcals.Length; i++)
            {
                cellPartcals[i].PlayEffectOut(outTime, startColor);
            }
        });
        sq.AppendInterval(outTime);
        sq.AppendCallback(() =>
        {
            for (int i = 0; i < cellPartcals.Length; i++)
            {
                cellPartcals[i].PlayEffectIn(inTime, endColor);
            }
            transform.DOPath(pos.ToArray(), inTime);
        });
        sq.AppendInterval(inTime);
        sq.AppendCallback(() => gameObject.SetActive(false));
    }

    [ContextMenu("Set up")]
    private void SetUp()
    {
        for (int i = 0; i < transform.childCount; i++)
        {
            var child = transform.GetChild(i);
            var pos = Random.insideUnitCircle * 1.5f;
            var scale = Random.Range(0.3f, 1f);
            var rotation = Random.Range(0f, 360f);
            child.localPosition = pos;
            child.transform.localScale = Vector3.one * scale;
            child.transform.Rotate(0f, 0f, rotation);
        }
    }
}