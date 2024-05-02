using System;
using UnityEngine;

namespace DarkcupGames
{
    public class MaxMediationBanner : MaxMediationAds
    {
        public string BANNER_ID;
        public Color bannerBackgroundColor;
        public MaxSdkBase.BannerPosition position = MaxSdkBase.BannerPosition.BottomCenter;

        public override void Init()
        {
            MaxSdkCallbacks.Banner.OnAdLoadedEvent += (arg1, info) =>
            {
                SetBannerVisible(true);
            };
        }

        public override void LoadAds()
        {
            MaxSdk.CreateBanner(BANNER_ID, position);
            MaxSdk.SetBannerBackgroundColor(BANNER_ID, bannerBackgroundColor);
        }

        public override void ShowAds(Action onShowAdsComplete, out bool available)
        {
            throw new NotImplementedException();
        }
        public  void ShowAds(Action onShowAdsComplete)
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
        }
    }
}