using System;
using UnityEngine;

namespace DarkcupGames
{
    public class MaxMediationBanner : MaxMediationAds
    {
        public string BANNER_ID;
        public Color bannerBackgroundColor;
        public MaxSdkBase.BannerPosition position = MaxSdkBase.BannerPosition.BottomCenter;
        private bool showing = false;
        private bool available = false;
        public override void Init()
        {
            MaxSdkCallbacks.Banner.OnAdLoadedEvent += (arg1, info) =>
            {
                CollapsibleBannerFlow.Instance.OnCollapsibleAdsLoaded();
                available = true;
            };
            MaxSdkCallbacks.Banner.OnAdLoadFailedEvent += (arg1, info) =>
            {
                available = false;
            };
        }

        public override void LoadAds()
        {
            MaxSdk.CreateBanner(BANNER_ID, position);
            MaxSdk.SetBannerBackgroundColor(BANNER_ID, bannerBackgroundColor);
        }

        public override void ShowAds(Action onShowAdsComplete)
        {
            SetBannerVisible(true);
        }

        public void SetBannerVisible(bool visible)
        {
            if (visible)
            {
                MaxSdk.ShowBanner(BANNER_ID);
            } else
            {
                MaxSdk.HideBanner(BANNER_ID);
            }
            showing = visible;
        }

        public override bool IsAdAvailable()
        {
            return available;
        }

        public override bool IsShowingAds()
        {
            return showing;
        }
    }
}