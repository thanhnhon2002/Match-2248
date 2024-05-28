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
        private float lastInterTime;

        private void Awake()
        {
            Instance = this;
        }
        private void Start()
        {
            lastInterTime = Time.time;
        }
        public void ShowIntertistial(string placement, Action onWatchAdsComplete)
        {
            var userData = GameSystem.userdata;
            if (Time.time < FirebaseManager.remoteConfig.MIN_SESSION_TIME_SHOW_ADS || userData.boughtItems.Contains(IAP_ID.no_ads.ToString()))
            {
                onWatchAdsComplete?.Invoke();
                return;
            }
            if (Time.time - lastInterTime < FirebaseManager.remoteConfig.TIME_BETWEEN_ADS) return;
            lastInterTime = Time.time;
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
                FirebaseManager.Instance.LogIntertisial(placement);
            });
        }

        public void ShowAds(int id)
        {
            var onWatchAdsFinished = events[id];
            var userData = GameSystem.userdata;
            if (userData.boughtItems.Contains(IAP_ID.no_ads.ToString()))
            {
                onWatchAdsFinished?.Invoke();
                return;
            }
            InternetChecker.Instance.CheckInternetConnection();
            if (InternetChecker.Instance.WasConnected == false) return;
            lastInterTime = Time.time;

            loadingAdPopup.SetActive(true);
            LeanTween.delayedCall(1f, () =>
            {
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