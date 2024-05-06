using System;
using UnityEngine;

namespace DarkcupGames
{
    public class MaxMediationReward : MaxMediationAds
    {
        public string REWARD_ID;
        private Action onShowAdsComplete;
        private int retryCount = 0;
        private bool isShowingAds;
        public static string placement;

        public override void Init()
        {
            MaxSdkCallbacks.Rewarded.OnAdLoadedEvent += OnRewardedAdLoadedEvent;
            MaxSdkCallbacks.Rewarded.OnAdLoadFailedEvent += OnRewardedAdLoadFailedEvent;
            MaxSdkCallbacks.Rewarded.OnAdHiddenEvent += OnRewardedAdHiddenEvent;
            MaxSdkCallbacks.Rewarded.OnAdReceivedRewardEvent += OnRewardedAdReceivedRewardEvent;
            MaxSdkCallbacks.Rewarded.OnAdRevenuePaidEvent += InRewardedPaidEvent;
            MaxSdkCallbacks.Rewarded.OnAdDisplayedEvent += OnAdDisplayedEvent;

        }

        private void OnAdDisplayedEvent(string arg1, MaxSdkBase.AdInfo info)
        {
            var impressionParameters = new[] {
                                        new Firebase.Analytics.Parameter("ad_platform", "AppLovin"),
                                        new Firebase.Analytics.Parameter("placement", placement), 
            };
            Firebase.Analytics.FirebaseAnalytics.LogEvent("rewarded_show_success", impressionParameters);
        }

        private void OnRewardedAdReceivedRewardEvent(string arg1, MaxSdkBase.Reward reward, MaxSdkBase.AdInfo info)
        {
            onShowAdsComplete?.Invoke();
        }

        private void OnRewardedAdHiddenEvent(string arg1, MaxSdkBase.AdInfo info)
        {
            LoadAds();
            isShowingAds = false;
        }

        private void OnRewardedAdLoadFailedEvent(string arg1, MaxSdkBase.ErrorInfo info)
        {
            retryCount++;
            float time = Mathf.Pow(2, retryCount);
            if (time > 64) time = 64;
            Invoke(nameof(LoadAds), time);
        }

        private void OnRewardedAdLoadedEvent(string arg1, MaxSdkBase.AdInfo info)
        {
            retryCount = 0;
        }

        private void InRewardedPaidEvent(string agr1, MaxSdkBase.AdInfo info)
        {
            AppsFlyerObjectScript.Instance.LogAdRevenue(info.NetworkName, info.AdUnitIdentifier, "rewarded", info.Placement, info.Revenue);
            double revenue = info.Revenue;
            var impressionParameters = new[] {
                                        new Firebase.Analytics.Parameter("ad_platform", "AppLovin"),
                                        new Firebase.Analytics.Parameter("ad_source", info.NetworkName),
                                        new Firebase.Analytics.Parameter("ad_unit_name", info.AdUnitIdentifier),
                                        new Firebase.Analytics.Parameter("ad_format", info.AdFormat),
                                        new Firebase.Analytics.Parameter("value", revenue),
                                        new Firebase.Analytics.Parameter("currency", "USD"), };
            Firebase.Analytics.FirebaseAnalytics.LogEvent("ad_impression", impressionParameters);
        }

        public override void LoadAds()
        {
            MaxSdk.LoadRewardedAd(REWARD_ID);
        }

        public override void ShowAds(Action onShowAdsComplete)
        {
            this.onShowAdsComplete = onShowAdsComplete;
            if (MaxSdk.IsRewardedAdReady(REWARD_ID) == false)
            {
                onShowAdsComplete?.Invoke();
                isShowingAds = false;
            } else
            {
                MaxSdk.ShowRewardedAd(REWARD_ID);
                isShowingAds = true;
            }

        }

        public override bool IsAdAvailable()
        {
            return MaxSdk.IsRewardedAdReady(REWARD_ID);
        }

        public override bool IsShowingAds()
        {
            return isShowingAds;
        }
    }
}