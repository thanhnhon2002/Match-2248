using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ISceneChangeEffect
{
    public void Prepare();
    public void RunEffect();
}

public class OnSceneChangeEffect : MonoBehaviour, ISceneChangeEffect
{
    public const float EFFECT_TIME = 1F;
    [SerializeField] private Vector3 startOffset;
    [SerializeField] private CanvasGroup group;
    [SerializeField] private bool move;
    [SerializeField] private bool fade;
    [SerializeField] private bool scale;

    [SerializeField] private Ease ease;
    private Vector3 basePosition;
    private Vector2 baseSizeDelta;
    private float baseAlpha;

    public void Prepare()
    {
        basePosition = transform.position;
        baseSizeDelta = ((RectTransform)transform).sizeDelta;
        if (group != null) baseAlpha = group.alpha;

        if (move) transform.position += startOffset;
        if (scale) ((RectTransform)transform).sizeDelta = Vector2.zero;
        if (fade && group != null) group.alpha = 0f;
    }

    public void RunEffect()
    {
        if (move) transform.DOMove(basePosition, EFFECT_TIME).SetEase(ease);
        if (scale) ((RectTransform)transform).DOSizeDelta(baseSizeDelta, EFFECT_TIME).SetEase(ease);
        if (fade && group != null) group.DOFade(baseAlpha, EFFECT_TIME).SetEase(ease);
    }
}
