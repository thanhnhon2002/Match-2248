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
            if (time > 64) time = 64;
            Invoke(nameof(LoadAds), time);
        }

        private void OnInterstitialHiddenEvent(string arg1, MaxSdkBase.AdInfo info)
        {
            onShowAdsComplete?.Invoke();
            LoadAds();
            isShowingAds = false;
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