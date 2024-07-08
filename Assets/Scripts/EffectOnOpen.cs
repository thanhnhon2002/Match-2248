using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EffectOnOpen : MonoBehaviour
{
    public readonly Vector2 START_SCALE = new Vector3(0.9f, 0.9f);
    public const float FADE_TIME = 0.3f;

    [SerializeField] private CanvasGroup parent;
    [SerializeField] private CanvasGroup[] fadeElenemt;
    [SerializeField] private float delayTime;

    private void OnEnable()
    {
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
        transform.DOScale(1f, FADE_TIME).SetEase(Ease.OutBack);
        parent.DOFade(1f, FADE_TIME);
        yield return new WaitForSeconds(FADE_TIME / 2);
        for (int i = 0; i < fadeElenemt.Length; i++)
        {
            fadeElenemt[i].DOFade(1f, FADE_TIME);
            yield return new WaitForSeconds(delayTime);
        }
    }
}
