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
        GetComponent<CanvasGroup>().DOFade(0, 0.35f);
        rectTransform.DOAnchorPosX(-Screen.width * value, fadeTime).SetEase(Ease.OutQuad).OnComplete(() =>
        {
            gameObject.SetActive(false);
        });
    }
}
