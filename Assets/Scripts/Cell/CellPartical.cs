using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using DarkcupGames;

public class CellPartical : MonoBehaviour
{
    public const float MIN_SIZE = 0.15f;
    public const float MAX_SIZE = 0.25f;
    public const float MIN_RANGE = 0.7f;
    public const float MAX_RANGE = 1.1f;

    public SpriteRenderer spriteRenderer;
    private Vector3 localPos;

    private void OnEnable()
    {
        transform.localPosition = Vector3.zero;
        transform.localScale = Vector3.zero;
        transform.localRotation = new Quaternion(0, 0, 0, 0);
        spriteRenderer.color = Color.clear;
    }

    public void PlayEffectOut(float time, Color startColor)
    {
        localPos = Random.insideUnitCircle * Random.Range(MIN_RANGE, MAX_RANGE);
        startColor.a = 0f;
        spriteRenderer.color = startColor;
        transform.localPosition = Vector3.zero;
        transform.localScale = Vector3.zero;
        transform.localRotation = new Quaternion(0, 0, 0, 0);
        spriteRenderer.DOFade(1f, time);
        transform.DOLocalMove(localPos, time).SetEase(Ease.OutCubic);
        transform.DORotate(new Vector3(0, 0, Random.Range(0, 360)), time);
        transform.DOScale(Vector3.one * Random.Range(MIN_SIZE, MAX_SIZE), time);
    }

    public void PlayEffectIn(float time, Color endColor)
    {
        transform.DOScale(0, time);
        transform.DOLocalMove(Vector3.zero, time * 0.5f).SetEase(Ease.Linear);
        transform.DOLocalRotate(Vector3.zero, time * 0.5f);
        spriteRenderer.DOFade(0f, time);
        spriteRenderer.DOColor(endColor, time);
    }
}