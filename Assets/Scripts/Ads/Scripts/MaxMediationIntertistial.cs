using System;
using UnityEngine;

namespace DarkcupGames
{
    public class MaxMediationIntertistial : MaxMediationAds
    {
        public string INTERTISTIAL_ID;
        private Action onShowAdsComplete;
        private int retryCount = 0;
        private bool isShowingAds;

        public override void Init()
        {
            MaxSdkCallbacks.Interstitial.OnAdLoadedEvent += OnInterstitialLoadedEvent;
            MaxSdkCallbacks.Interstitial.OnAdLoadFailedEvent += OnInterstitialLoadFailedEvent;
            MaxSdkCallbacks.Interstitial.OnAdHiddenEvent += OnInterstitialHiddenEvent;
            MaxSdkCallbacks.Interstitial.OnAdRevenuePaidEvent += InInterstitialPaidEvent;
            MaxSdkCallbacks.Interstitial.OnAdDisplayedEvent += OnAdDisplayedEvent;
        }

        private void OnInterstitialLoadedEvent(string arg1, MaxSdkBase.AdInfo info)
        {
            retryCount = 0;
        }

        private void OnInterstitialLoadFailedEvent(string arg1, MaxSdkBase.ErrorInfo info)
        {
            if (showDebug)
            {
                Debug.LogError("load intertistial failed");
                Debug.LogError(info.Message);
            }
            retryCount++;
            float time = Mathf.Pow(2, retryCount);
            if (time > AdManagerMax.MAX_RETRY_TIME) time = AdManagerMax.MAX_RETRY_TIME;
            Invoke(nameof(LoadAds), time);
        }

        private void OnInterstitialHiddenEvent(string arg1, MaxSdkBase.AdInfo info)
        {
            onShowAdsComplete?.Invoke();
            LoadAds();
            isShowingAds = false;
        }

        private void OnAdDisplayedEvent(string arg1, MaxSdkBase.AdInfo info)
        {
            var impressionParameters = new[] {
                                        new Firebase.Analytics.Parameter("ad_platform", "AppLovin"),
                                        new Firebase.Analytics.Parameter("placement", "Gameplay"),
            };
            Firebase.Analytics.FirebaseAnalytics.LogEvent("inter_show_success", impressionParameters);
        }

        private void InInterstitialPaidEvent(string agr1, MaxSdkBase.AdInfo info)
        {
            AppsFlyerObjectScript.Instance.LogAdRevenue(info.NetworkName, info.AdUnitIdentifier, "intertitial", info.Placement, info.Revenue);
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
            MaxSdk.LoadInterstitial(INTERTISTIAL_ID);
        }

        public override void ShowAds(Action onShowAdsComplete)
        {
            this.onShowAdsComplete = onShowAdsComplete;
            if (MaxSdk.IsInterstitialReady(INTERTISTIAL_ID) == false)
            {
                onShowAdsComplete?.Invoke();
                isShowingAds = false;
            } else
            {
                MaxSdk.ShowInterstitial(INTERTISTIAL_ID);
                isShowingAds = true;
            }
        }

        public override bool IsAdAvailable()
        {
            return MaxSdk.IsInterstitialReady(INTERTISTIAL_ID);
        }

        public override bool IsShowingAds()
        {
            return isShowingAds;
        }
    }
}