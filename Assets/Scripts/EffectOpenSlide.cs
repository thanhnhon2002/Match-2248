using DG.Tweening;
using UnityEngine;

public class EffectOpenSlide: MonoBehaviour
{
    [SerializeField] private float fadeTime = 0.35f;
    private RectTransform rectTransform;

    private void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
    }

    private void OnEnable()
    {
        rectTransform = GetComponent<RectTransform>();
        DoEffect();
    }

    private void DoEffect()
    {
        DOTween.Kill(rectTransform);
        rectTransform.anchoredPosition = new Vector2(Screen.width, rectTransform.anchoredPosition.y);
        rectTransform.DOAnchorPosX(0, fadeTime).SetEase(Ease.OutQuad);
    }
}