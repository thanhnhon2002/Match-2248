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
        private MaxMediationIntertistial intertistial;
        private MaxMediationReward rewarded;
        public bool isCurrentAdAvaiable;

        private void Awake()
        {
            Instance = this;
        }
        public void ShowIntertistial(Action onWatchAdsComplete, out bool available)
        {
            MaxMediationManager.intertistial.ShowAds(onWatchAdsComplete, out available);
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
                }, out isCurrentAdAvaiable);
            });

        }
    }
}