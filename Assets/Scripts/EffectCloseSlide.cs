using DG.Tweening;
using System;
using UnityEngine;

public class EffectCloseSlide : MonoBehaviour
{
    private RectTransform rectTransform;
    [SerializeField] private float fadeTime = 0.35f;
    public void Close(bool isReverse = false)
    {
        var value = isReverse ? -1f : 1f;
        if (rectTransform == null) rectTransform = GetComponent<RectTransform>();
        DOTween.Kill(rectTransform);
        rectTransform.DOAnchorPosX(-Screen.width * 2 * value, fadeTime).SetEase(Ease.OutQuad).OnComplete(() =>
        {
            gameObject.SetActive(false);
        });
    }
}
