using DarkcupGames;
using GoogleMobileAds.Api;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

public class AdmobMREC : AdmobAds
{
    public string MREC_ID;
    [HideInInspector] public BannerView bannerView { get; protected set; }
    private string uuid;
    private bool available;
    private bool isShowing;
    public override void Init()
    {
        bannerView = new BannerView(MREC_ID, AdSize.MediumRectangle, AdPosition.Top);
        // Raised when an ad is loaded into the banner view.
        bannerView.OnBannerAdLoaded += () =>
        {
            available = true;
        };
        // Raised when an ad fails to load into the banner view.
        bannerView.OnAdPaid += (AdValue adValue) => AppsFlyerObjectScript.Instance.LogAdRevenue("admob", bannerView.GetAdUnitID(), "MREC", string.Empty, adValue.Value);
        bannerView.OnAdPaid += (AdValue value) => AdjustLog.OnAdRevenuePaidEventAdmob(value);
        bannerView.OnBannerAdLoadFailed += (LoadAdError error) =>
        {
            Debug.LogError("MREC view failed to load an ad with error : "
                + error);
        };
    }

    public override bool IsAdsAvailable()
    {
        return available;
    }

    public override bool IsShowingAds()
    {
        return isShowing;
    }

    public override void LoadAds()
    {
        try
        {
            if (AdmobManager.isReady == false)
            {
                AdmobManager.Instance.Init();
                Debug.LogError("admob is not ready for load MREC");
                return;
            }
            var adRequest = new AdRequest();
            bannerView.LoadAd(adRequest);
            bannerView.Hide();
        } catch (Exception e) { }
    }

    public override bool ShowAds(Action onShowAdsComplete)
    {
        SetBannerVisible(available);
        return available;
    }

    public void GenerateNewUUID()
    {
        uuid = Guid.NewGuid().ToString();
    }

    public void SetBannerVisible(bool visible)
    {
        if (bannerView == null) return;
        if (visible)
        {
            bannerView.Show();
            isShowing = true;
        } else
        {
            bannerView.Hide();
            isShowing = false;
        }
    }

    public void HideAds()
    {
        SetBannerVisible(false);
    }
}
