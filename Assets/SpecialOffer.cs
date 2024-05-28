using DarkcupGames;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class SpecialOffer : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI oldPrice;
    public PopupAnimation popup;
    private void OnEnable()
    {
        DisplayOldPrice();
    }

    private void DisplayOldPrice()
    {
        if (ShopIAPManager.Instance == null) return;
        if (ShopIAPManager.Instance.IsInitDone() == false) return;
        Dictionary<string, string> prices = ShopIAPManager.iap.prices;
        if (prices.ContainsKey(IAP_ID.no_ads.ToString()) == false) return;
        oldPrice.text = "<s>" + prices[IAP_ID.no_ads.ToString()] + "</s>";
    }

    public void BuyNoAdSpecial()
    {
        ShopIAPManager.Instance.BuyNoAdsSpecial();
    }
}
