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
        public float interInterval = 300f;

        private void Awake()
        {
            Instance = this;
        }
        public void ShowIntertistial(Action onWatchAdsComplete)
        {
            adBreak.gameObject.SetActive(true);
            LeanTween.delayedCall(1f, () =>
            {
                adBreak.gameObject.SetActive(false);
                MaxMediationManager.intertistial.ShowAds(onWatchAdsComplete);
            });
        }

        public void ShowAds(int id)
        {
            bool haveAds = MaxMediationManager.rewarded.IsAdAvailable();

            loadingAdPopup.SetActive(true);
            LeanTween.delayedCall(1f, () =>
            {
                loadingAdPopup.SetActive(false);
                var onWatchAdsFinished = events[id];
                MaxMediationManager.rewarded.ShowAds(() =>
                {
                    onWatchAdsFinished?.Invoke();
                });
            });
        }
    }
}