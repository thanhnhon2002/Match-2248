using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace DarkcupGames
{
    public class AdManagerMax : MonoBehaviour
    {
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
        public void ShowIntertistial(Action onWatchAdsComplete)
        {
            onWatchAdsComplete += ()=> adBreak.gameObject.SetActive(false);
            adBreak.gameObject.SetActive(true);
            LeanTween.delayedCall(1f, () =>
            {           
                MaxMediationManager.intertistial.ShowAds(onWatchAdsComplete);
                GameSystem.userdata.property.total_interstitial_ads++;
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
                    loadingAdPopup.SetActive(false);
                    onWatchAdsFinished?.Invoke();
                });
                GameSystem.userdata.property.total_rewarded_ads++;
                FirebaseManager.Instance.SetProperty(UserPopertyKey.total_rewarded_ads, GameSystem.userdata.property.total_rewarded_ads.ToString());
            });
        }
    }
}