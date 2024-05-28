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
        float posX = group.transform.position.x;
        group.transform.position = new Vector3(posX - 100f, transform.position.y, transform.position.z);
        group.transform.DOMoveX(posX, Const.DEFAULT_TWEEN_TIME + 1f).SetEase(Ease.OutQuad);
        group.alpha = 0.1f;
        group.DOFade(1f, Const.DEFAULT_TWEEN_TIME + 1f).OnComplete(() => button.interactable = true);
        timeCounter.ResetTimer();
        timeCounter.SetTime();
    }

    public void Close()
    {
        button.interactable = false;
        group.DOFade(0f, Const.DEFAULT_TWEEN_TIME).OnComplete(() => gameObject.SetActive(false));
    }
}
