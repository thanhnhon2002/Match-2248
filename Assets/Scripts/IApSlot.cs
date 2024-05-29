using DarkcupGames;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IApSlot : MonoBehaviour
{
    public IAP_ID apid;
    [SerializeField] private ShopIAPManager shop;
    public void OnClick()
    {
        if (apid == IAP_ID.no_ads) shop.BuyNoAdsPackage();
        else
        {
            shop.clickedButton = transform;
            shop.BuyDiamond(apid);
        }
    }
}
