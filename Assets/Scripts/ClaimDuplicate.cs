using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClaimDuplicate : MonoBehaviour
{
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
        PopupManager.Instance.DeQueue();
        GameSystem.SaveUserDataToLocal();
    }
}
