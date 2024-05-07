using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClaimDuplicate : MonoBehaviour
{
    [SerializeField] private PopupAnimation popup;
    [SerializeField] private float cost;
    public void OnClick()
    {
        var userData = GameSystem.userdata;
        if (userData.diamond < cost)
        {
            GameFlow.Instance.shop.SetActive(true);
            return;
        }
        userData.diamond -= cost;
        GameFlow.Instance.diamondGroup.Display();
        GridManager.Instance.DoubleHightCellValue();
        popup.Disappear();
        PopupManager.Instance.DeQueue();
        GameSystem.SaveUserDataToLocal();
    }
}
