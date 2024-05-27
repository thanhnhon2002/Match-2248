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

    private void Start()
    {
        Instance = this;
        Display();
    }

    public void AddDiamond(int amount, bool doEffect = false)
    {
        var userData = GameSystem.userdata;
        DOVirtual.Float(userData.diamond, userData.diamond + amount, Const.DEFAULT_TWEEN_TIME, x =>
        {
            userData.diamond = x;
            diamondTxt.text = "" + x;
        }).OnComplete(GameSystem.SaveUserDataToLocal);
        if (!doEffect) return;
        EasyEffect.Bounce(icon.gameObject, 0.1f);
    }
    public void Display()
    {
        var userData = GameSystem.userdata;
        diamondTxt.text = userData.diamond.ToString();
    }
}
