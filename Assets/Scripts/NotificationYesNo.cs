using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class NotificationYesNo : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI message;
    [SerializeField] private Button btnNo;
    [SerializeField] private Button btnYes;
    [SerializeField] private Image maskImg;
    private CanvasGroup canvasGroup;

    private void Awake()
    {
        btnNo.onClick.AddListener(Hide);
        canvasGroup = GetComponent<CanvasGroup>();
    }
    public void Show(string messageInput, Action actionClickBtnYes, Action actionClickBtnNo)
    {
        maskImg.gameObject.SetActive(true);
        message.text = "";
        message.text = messageInput;
        btnYes.onClick.RemoveAllListeners();
        btnNo.onClick.RemoveAllListeners();
        btnYes.onClick.AddListener(() => 
        {
            actionClickBtnYes?.Invoke();
            Hide();
        });
        btnNo.onClick.AddListener(() =>
        {
            actionClickBtnNo?.Invoke();
            Hide();
        });
        transform.DOScale(1, 0.2f).SetEase(Ease.OutBack);
        canvasGroup.DOFade(1, 0.2f);
    }

    public void Hide()
    {
        canvasGroup.DOFade(0, 0.2f);
        transform.DOScale(0.5f, 0.2f).SetEase(Ease.InBack);
        maskImg.DOFade(0, 0.2f).OnComplete(()=> 
        {
            maskImg.gameObject.SetActive(false);
        });
    }
}
