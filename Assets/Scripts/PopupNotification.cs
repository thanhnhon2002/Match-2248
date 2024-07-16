using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PopupNotification : MonoBehaviour
{
    public static PopupNotification Instance;

    [SerializeField] NotificationYesNo popupYesNo;
    private void Awake()
    {
        Instance = this;
    }

    public void ShowPopupYesNo(string message, Action onClickBtnYes, Action actionClickBtnNo = null)
    {
        popupYesNo.gameObject.SetActive(true);
        popupYesNo.Show(message, onClickBtnYes, actionClickBtnNo);
    }
}
