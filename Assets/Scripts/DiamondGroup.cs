using DarkcupGames;
using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Numerics;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DiamondGroup : MonoBehaviour
{
    public static DiamondGroup Instance;
    [SerializeField] private Image icon;
    [SerializeField] private TextMeshProUGUI diamondTxt;
    private float oldDiamondAmount;

    private void Start()
    {
        Instance = this;
        Display();
    }

    public void AddDiamond(int amount)
    {
        var userData = GameSystem.userdata;
        oldDiamondAmount = userData.diamond;
        userData.diamond += amount;
        GameSystem.SaveUserDataToLocal();
        DataUserManager.SaveUserData();
    }

    public void Display()
    {
        var userData = GameSystem.userdata;
        if(userData.diamond < 0) userData.diamond = 0;
        GameSystem.SaveUserDataToLocal();
        DOVirtual.Float((int)oldDiamondAmount, (int)userData.diamond, 0.8f, x =>
        {
            diamondTxt.text = (int)x+"";
        }).OnComplete(() =>
        {
            diamondTxt.text = MoneyConveter.ConvertNameValueBestCode((int)userData.diamond);
        });
        EasyEffect.Bounce(icon.gameObject, 0.1f);
    }
}