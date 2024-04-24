using DarkcupGames;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DiamondGroup : MonoBehaviour
{
    [SerializeField] private Image icon;
    [SerializeField] private TextMeshProUGUI diamondTxt;

    private void Start ()
    {
        diamondTxt.text = GameSystem.userdata.diamond.ToString ();
    }

    public void AddDiamond(int amount, bool doEffect = false)
    {
        var userData = GameSystem.userdata;
        userData.diamond += amount;
        GameSystem.SaveUserDataToLocal ();
        diamondTxt.text = userData.diamond.ToString ();
        if (!doEffect) return;
        EasyEffect.Bounce (icon.gameObject, 0.1f) ;
    }
}
