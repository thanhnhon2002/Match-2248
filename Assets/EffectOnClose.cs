using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EffectOnClose : MonoBehaviour
{
    [SerializeField] private CanvasGroup parent;
    [SerializeField] private float fadeTime;
    public void Close()
    {
        parent.DOFade(0, Const.DEFAULT_TWEEN_TIME);
        transform.DOScale(0.5f, Const.DEFAULT_TWEEN_TIME).SetEase(Ease.InBack).OnComplete(() => 
        {
            gameObject.SetActive(false);
            transform.localScale = Vector3.one;
        });
    }
}
