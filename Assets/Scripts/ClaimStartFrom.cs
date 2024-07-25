using DarkcupGames;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ClaimStartFrom : MonoBehaviour
{
    private float cost;
    [SerializeField] private RewardEventLog logger;
    public Button button;
    [SerializeField] private Image adIcon;
    private bool isAd;

    public bool IsAd
    {
        get { return isAd; }
        set {
            isAd = value;
            adIcon.gameObject.SetActive(value);
        }
    }

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

    public void OnClickCheckAd()
    {
        if(IsAd)
        {
            if (logger != null) logger.LogEvent();
            AdManagerMax.Instance.ShowAds(6);
            return;
        }
        Onclick();
    }

    public void Onclick()
    {
        var userData = GameSystem.userdata;
        if (userData.diamond < cost)
        {
            GameFlow.Instance.shop.SetActive(true);
            GameFlow.Instance.shop.GetComponent<EffectOpenSlide>().DoEffect();
            return;
        }
       
        GameFlow.Instance.diamondGroup.AddDiamond((int)-cost);
        GameSystem.SaveUserDataToLocal();
        transform.parent.parent.GetComponent<AnimCombo>().CloseAnim();
        DataUserManager.SaveUserData();
        GameFlow.Instance.diamondGroup.Display();
        GridManager.Instance.SetStartIndex();
    }

}
