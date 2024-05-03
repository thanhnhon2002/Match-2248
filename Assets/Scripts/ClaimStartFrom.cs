using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClaimStartFrom : MonoBehaviour
{
    public float cost;
    [SerializeField] private PopupAnimation popup;
    public void Onclick()
    {
        var userData = GameSystem.userdata;
        if (userData.diamond < cost) return;
        userData.diamond -= cost;
        GameSystem.SaveUserDataToLocal();
        GameFlow.Instance.diamondGroup.Display();
        popup.Disappear();
        GridManager.Instance.SetStartIndex();
    }
}
