using System;
using UnityEngine;

namespace DarkcupGames
{
    public class MaxMediationReward : MaxMediationAds
    {
        public string REWARD_ID;
        private Action onShowAdsComplete;
        private int retryCount = 0;
        public override void Init()
        {
            MaxSdkCallbacks.Rewarded.OnAdLoadedEvent += OnRewardedAdLoadedEvent;
            MaxSdkCallbacks.Rewarded.OnAdLoadFailedEvent += OnRewardedAdLoadFailedEvent;
            MaxSdkCallbacks.Rewarded.OnAdHiddenEvent += OnRewardedAdHiddenEvent;
            MaxSdkCallbacks.Rewarded.OnAdReceivedRewardEvent += OnRewardedAdReceivedRewardEvent;
        }

        private void OnRewardedAdReceivedRewardEvent(string arg1, MaxSdkBase.Reward reward, MaxSdkBase.AdInfo info)
        {
            onShowAdsComplete?.Invoke();
        }

        private void OnRewardedAdHiddenEvent(string arg1, MaxSdkBase.AdInfo info)
        {
            LoadAds();
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
            } else
            {
                MaxSdk.ShowRewardedAd(REWARD_ID);
            }

        }

        public override bool IsAdAvailable()
        {
            return MaxSdk.IsRewardedAdReady(REWARD_ID);
        }
    }
}