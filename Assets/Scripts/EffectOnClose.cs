using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EffectOnClose : MonoBehaviour
{
    public readonly Vector2 END_SCALE = new Vector3(0.98f, 0.98f);

    [SerializeField] private CanvasGroup parent;
    [SerializeField] private float fadeTime;
    private RectTransform rectTransform;
    private void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
    }
    public void Close()
    {
        if (rectTransform == null) rectTransform = GetComponent<RectTransform>();
        if (Const.USE_MOVE_ANIMATION_HOME)
        {
            rectTransform.DOAnchorPosX(-Screen.width, fadeTime).SetEase(Ease.OutCubic).OnComplete(() =>
            {
                gameObject.SetActive(false);
            });
            return;
        }

        parent.DOFade(0, fadeTime);
        transform.DOScale(END_SCALE, fadeTime).SetEase(Ease.InBack).OnComplete(() => 
        {
            gameObject.SetActive(false);
            transform.localScale = Vector3.one;
        });
    }
}
