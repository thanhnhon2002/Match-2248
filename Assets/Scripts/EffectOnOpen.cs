using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EffectOnOpen : MonoBehaviour
{
    public readonly Vector2 START_SCALE = new Vector3(0.98f, 0.98f);

    [SerializeField] private CanvasGroup parent;
    [SerializeField] private CanvasGroup[] fadeElenemt;
    [SerializeField] private float fadeTime;
    [SerializeField] private float delayTime;
    private RectTransform rectTransform;

    private void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
    }

    private void OnEnable()
    {
        if (Const.USE_MOVE_ANIMATION_HOME)
        {
            rectTransform.anchoredPosition = new Vector2(Screen.width, rectTransform.anchoredPosition.y);
            rectTransform.DOAnchorPosX(0, fadeTime).SetEase(Ease.OutCubic);
            return;
        }

        transform.localScale = START_SCALE;
        parent.alpha = 0f;
        for (int i = 0; i < fadeElenemt.Length; i++)
        {
            fadeElenemt[i].alpha = 0f;
        }
        StartCoroutine(DoEffect());
    }

    private IEnumerator DoEffect()
    {
        transform.DOScale(1f, fadeTime).SetEase(Ease.OutBack);
        parent.DOFade(1f, fadeTime);
        yield return new WaitForSeconds(fadeTime);
        for (int i = 0; i < fadeElenemt.Length; i++)
        {
            fadeElenemt[i].DOFade(1f, fadeTime);
            yield return new WaitForSeconds(delayTime);
        }
    }
}
