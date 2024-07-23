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
        public const float INTER_BUFFER = 30F;
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
            MaxMediationManager.intertistial.AddOnAdCloseAction((str, info) =>
            {
                HideAdBreak();
            });
            MaxMediationManager.intertistial.AddOnAdFailAction((str, inof) =>
            {
                HideAdBreak();
            });


            MaxMediationManager.rewarded.AddOnAdCloseAction((str, info) =>
            {
                HideAdLoading();
            });
            MaxMediationManager.rewarded.AddOnAdFailAction((str, info) =>
            {
                HideAdLoading();
            });
        }

        private void HideAdLoading()
        {
            if (loadingAdPopup != null) loadingAdPopup.SetActive(false);
        }

        private void HideAdBreak()
        {
            if (adBreak != null) adBreak.gameObject.SetActive(false);
        }

        public void ShowIntertistial(string placement, Action onWatchAdsComplete)
        {
            onWatchAdsComplete += () =>
            {
                if (adBreak != null)
                {
                    GameSystem.userdata.diamond += adBreak.diamondAmount;
                    GameSystem.SaveUserDataToLocal();
                    DataUserManager.SaveUserData();
                }
            };
            var userData = GameSystem.userdata;
            userData.CheckValid();
            if (Time.time < FirebaseManager.remoteConfig.MIN_SESSION_TIME_SHOW_ADS || userData.boughtItems.Contains(IAP_ID.no_ads.ToString()))
            {
                Debug.Log("Time.time: " + Time.time);
                onWatchAdsComplete?.Invoke();
                return;
            }
            if (Time.time - lastInterTime < FirebaseManager.remoteConfig.TIME_BETWEEN_ADS)
            {
                Debug.Log("Time.time - lastInterTime:" + (Time.time - lastInterTime));
                onWatchAdsComplete?.Invoke();
                return;
            }
            lastInterTime = Time.time;
            bool haveAds = MaxMediationManager.intertistial.IsAdAvailable();
            if (haveAds == false || GameSystem.userdata.boughtItems.Contains(IAP_ID.no_ads.ToString()))
            {
                Debug.Log("haveAds" + haveAds);
                onWatchAdsComplete?.Invoke();
                return;
            }
            onWatchAdsComplete += () =>
            {
                DeepTrack.LogEvent(DeepTrackEvent.inter_success);
            };
            if (adBreak != null) adBreak.gameObject.SetActive(true);
            LeanTween.delayedCall(DELAY_SHOW_INTER, () =>
            {
                MaxMediationManager.intertistial.ShowAds(onWatchAdsComplete);
                GameSystem.userdata.property.total_interstitial_ads++;
                GameSystem.SaveUserDataToLocal();
                FirebaseManager.Instance.SetProperty(UserPopertyKey.total_interstitial_ads, GameSystem.userdata.property.total_interstitial_ads.ToString());
                FirebaseManager.Instance.LogIntertisial(placement);
                Debug.Log("ShowAds");
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
            lastInterTime = Time.time + INTER_BUFFER;

            if (loadingAdPopup != null) loadingAdPopup.SetActive(true);
            LeanTween.delayedCall(1f, () =>
            {
                MaxMediationManager.rewarded.ShowAds(() =>
                {
                    DeepTrack.LogEvent(DeepTrackEvent.reward_success);
                    onWatchAdsFinished?.Invoke();
                });
                GameSystem.userdata.property.total_rewarded_ads++;
                FirebaseManager.Instance.SetProperty(UserPopertyKey.total_rewarded_ads, GameSystem.userdata.property.total_rewarded_ads.ToString());
            });
        }
    }
}