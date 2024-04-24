using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AnimationPanelPause : AnimationPanel
{
    private readonly WaitForSeconds wait = new WaitForSeconds (0.1f);
    [SerializeField] private CanvasGroup[] buttons;
    [SerializeField] private CanvasGroup group;
    public override void Animation ()
    {
        group.alpha = 0f;
        transform.localScale = Vector3.zero;
        foreach (var item in buttons)
        {
            item.alpha = 0f;
        }
        group.DOFade (1f, Const.DEFAULT_TWEEN_TIME);
        transform.DOScale (1f, Const.DEFAULT_TWEEN_TIME).OnComplete (() => StartCoroutine (ShowButtons ()));
    }

    private IEnumerator ShowButtons()
    {
        foreach (var item in buttons)
        {
            item.DOFade (1f, 0.3f);
            yield return wait;
        }
    }
}
