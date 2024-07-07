using DG.Tweening;
using UnityEngine;

public class EffectCloseSlide : MonoBehaviour
{
    private RectTransform rectTransform;
    [SerializeField] private float fadeTime = 0.35f;
    public void Close()
    {
        if (rectTransform == null) rectTransform = GetComponent<RectTransform>();
        DOTween.Kill(rectTransform);
        rectTransform.DOAnchorPosX(-Screen.width * 2, fadeTime).SetEase(Ease.OutQuad).OnComplete(() =>
        {
            gameObject.SetActive(false);
        });
    }
}