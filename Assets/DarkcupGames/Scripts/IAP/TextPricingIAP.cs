using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using DarkcupGames;

public class TextPricingIAP : MonoBehaviour
{
    public TextMeshProUGUI txt;
    public IAP_ID id;

    private void Awake()
    {
        txt = GetComponent<TextMeshProUGUI>();
    }

    private void OnEnable()
    {
        UpdateDisplay();
    }

    public void UpdateDisplay()
    {
        if (ShopIAPManager.Instance == null) return;
        if (ShopIAPManager.Instance.IsInitDone() == false) return;
        Dictionary<string, string> prices = ShopIAPManager.iap.prices;
        if (prices.ContainsKey(id.ToString()) == false) return;
        txt.text = prices[id.ToString()];
    }
}