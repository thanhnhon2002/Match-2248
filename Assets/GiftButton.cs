using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GiftButton : MonoBehaviour
{
    [SerializeField] private CanvasGroup group;
    [SerializeField] private Button button;
    [SerializeField] private TimeCounter timeCounter;

    private void OnEnable()
    {
        group.alpha = 0f;
        group.DOFade(1f, Const.DEFAULT_TWEEN_TIME).OnComplete(() => button.interactable = true);
        timeCounter.ResetTimer();
        timeCounter.SetTime();
    }

    public void Close()
    {
        button.interactable = false;
        group.DOFade(0f, Const.DEFAULT_TWEEN_TIME).OnComplete(() => gameObject.SetActive(false));
    }
}
