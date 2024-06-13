using GoogleMobileAds.Api;
using System;
using UnityEngine;
using DarkcupGames;
using Firebase.Analytics;
using UnityEngine.SceneManagement;

public class AdmobAdBanner : AdmobAds
{
    public string BANNER_ID;
    public bool useCollapsible = true;
    public AdPosition position = AdPosition.Bottom;
    private BannerView bannerView;
    private string uuid;
    private bool available;
    private bool isShowingAds;

    public override void Init()
    {
        bannerView = new BannerView(BANNER_ID, AdSize.Banner, position);
        bannerView.OnBannerAdLoaded += () =>
        {
            available = true;
            CollapsibleBannerFlow.Instance.OnCollapsibleAdsLoaded();
        };
        bannerView.OnBannerAdLoadFailed += (err) =>
        {
            Debug.LogError("load banner failed");
            Debug.LogError(err.GetMessage());
            available = false;
            CollapsibleBannerFlow.Instance.OnCollapsibleAdsFailed();
        };
        bannerView.OnAdPaid += (AdValue adValue) => AppsFlyerObjectScript.Instance.LogAdRevenue("admob", bannerView.GetAdUnitID(), "default", string.Empty, adValue.Value);
        bannerView.OnAdImpressionRecorded += () => 
        {
            var param = new Parameter[]
            {
                    new Parameter("ad_platform", "admob"),
                    new Parameter("placement", "default"),
            };
            FirebaseAnalytics.LogEvent("collap_banner_show_success", param);
        };
        GenerateNewUUID();
    }

    public override void LoadAds()
    {
        try
        {
            if (AdmobManager.isReady == false)
            {
                AdmobManager.Instance.Init();
                Debug.LogError("admob is not ready for load banner");
                return;
            }
            var adRequest = new AdRequest();
            if (useCollapsible && !SceneManager.GetActiveScene().name.Equals("GameplayUI"))
            {
                adRequest.Extras.Add("collapsible", "bottom");
                adRequest.Extras.Add("collapsible_request_id", uuid);
            }
            bannerView.LoadAd(adRequest);
        }
        catch (Exception e) { }
    }

    public override bool ShowAds(Action onShowAdsComplete)
    {
        SetBannerVisible(available);
        return available;
    }
    public void SetBannerVisible(bool visible)
    {
        if (bannerView == null) return;
        if (visible)
        {
            bannerView.Show();
            isShowingAds = true;
        } else
        {
            bannerView.Hide();
            isShowingAds = false;
        }
    }

    [ContextMenu("Test")]
    public void GenerateNewUUID()
    {
        uuid = Guid.NewGuid().ToString();
    }

    public override bool IsAdsAvailable()
    {
        return available;
    }

    public override bool IsShowingAds()
    {
        return isShowingAds;
    }
}