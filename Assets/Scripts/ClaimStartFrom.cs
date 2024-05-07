using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ClaimStartFrom : MonoBehaviour
{
    private float cost;
    [SerializeField] private PopupAnimation popup;
    [SerializeField] private Button button;

    public float Cost
    { 
        get { return cost; } 
        set 
            {
            cost = value;
            var userData = GameSystem.userdata;
            button.interactable = userData.diamond >= cost;
            } 
    }

    public void Onclick()
    {
        var userData = GameSystem.userdata;
        if (userData.diamond < cost)
        {
            GameFlow.Instance.shop.SetActive(true);
            return;
        }
        userData.diamond -= cost;
        GameSystem.SaveUserDataToLocal();
        GameFlow.Instance.diamondGroup.Display();
        popup.Disappear();
        GridManager.Instance.SetStartIndex();
    }
}
