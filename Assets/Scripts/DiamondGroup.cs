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

    public void AddDiamond(int amount, bool doEffect = false)
    {
        var userData = GameSystem.userdata;
        oldDiamondAmount = userData.diamond;
        userData.diamond += amount;
        GameSystem.SaveUserDataToLocal ();
        //if (!doEffect) return;
       
    }

    public void AddDiamond(int amount)
    {
        var userData = GameSystem.userdata;
        oldDiamondAmount = userData.diamond;
        userData.diamond += amount;
        GameSystem.SaveUserDataToLocal();
        //EasyEffect.Bounce(icon.gameObject, 0.1f);
    }

    public void Display()
    {
        var userData = GameSystem.userdata;
        if(userData.diamond < 0) userData.diamond = 0;
        GameSystem.SaveUserDataToLocal();
        DOVirtual.Float(oldDiamondAmount, userData.diamond, Const.DEFAULT_TWEEN_TIME, x =>
        {
            diamondTxt.text = MoneyConveter.ConvertNameValueBestCode((int)x);
        });
        EasyEffect.Bounce(icon.gameObject, 0.1f);
    }
}
