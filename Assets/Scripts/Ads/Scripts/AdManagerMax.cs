using DeepTrackSDK;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;

namespace DarkcupGames
{
    public class AdManagerMax : MonoBehaviour
    {
        private const float DELAY_SHOW_INTER = 0.5f;
        public static readonly float MAX_RETRY_TIME = 64f;
        public static AdManagerMax Instance { get; private set; }
        public List<UnityEvent> events;
        [SerializeField] private GameObject loadingAdPopup;
        [SerializeField] private AdBreak adBreak;
        private MaxMediationIntertistial intertistial;
        private MaxMediationReward rewarded;
        public float lastInterTime;

        private void Awake()
        {
            Instance = this;
        }
        private void Start()
        {
            lastInterTime = Time.time;
        }
        public void ShowIntertistial(Action onWatchAdsComplete)
        {
            if (Time.time < FirebaseManager.remoteConfig.MIN_SESSION_TIME_SHOW_ADS)
            {
                onWatchAdsComplete?.Invoke();
                return;
            }
            bool haveAds = MaxMediationManager.intertistial.IsAdAvailable();
            if (haveAds == false || GameSystem.userdata.boughtItems.Contains(IAP_ID.no_ads.ToString()))
            {
                onWatchAdsComplete?.Invoke();
                return;
            }
            onWatchAdsComplete += () =>
            {
                DeepTrack.LogEvent(DeepTrackEvent.inter_success);
                adBreak.gameObject.SetActive(false);
            };
            adBreak.gameObject.SetActive(true);
            LeanTween.delayedCall(DELAY_SHOW_INTER, () =>
            {
                MaxMediationManager.intertistial.ShowAds(onWatchAdsComplete);
                GameSystem.userdata.property.total_interstitial_ads++;
                GameSystem.SaveUserDataToLocal();
                FirebaseManager.Instance.SetProperty(UserPopertyKey.total_interstitial_ads, GameSystem.userdata.property.total_interstitial_ads.ToString());
            });
        }

        public void ShowAds(int id)
        {
            loadingAdPopup.SetActive(true);
            LeanTween.delayedCall(1f, () =>
            {
                var onWatchAdsFinished = events[id];
                MaxMediationManager.rewarded.ShowAds(() =>
                {
                    DeepTrack.LogEvent(DeepTrackEvent.reward_success);
                    loadingAdPopup.SetActive(false);
                    onWatchAdsFinished?.Invoke();
                });
                GameSystem.userdata.property.total_rewarded_ads++;
                FirebaseManager.Instance.SetProperty(UserPopertyKey.total_rewarded_ads, GameSystem.userdata.property.total_rewarded_ads.ToString());
            });
        }
    }
}