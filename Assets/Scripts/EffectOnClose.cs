using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EffectOnClose : MonoBehaviour
{
    public readonly Vector2 END_SCALE = new Vector3(0.9f, 0.9f);
    public const float FADE_TIME = 0.3f;

    [SerializeField] private CanvasGroup parent;

    public void Close()
    {
        parent.DOFade(0, FADE_TIME);
        transform.DOScale(END_SCALE, FADE_TIME).SetEase(Ease.InBack).OnComplete(() => 
        {
            gameObject.SetActive(false);
            transform.localScale = Vector3.one;
        });
    }
}
