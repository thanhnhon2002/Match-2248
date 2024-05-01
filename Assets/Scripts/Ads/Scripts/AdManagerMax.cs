using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace DarkcupGames
{
    public class AdManagerMax : MonoBehaviour
    {
        public List<UnityEvent> events;
        private MaxMediationIntertistial intertistial;
        private MaxMediationReward rewarded;

        public void ShowIntertistial(Action onWatchAdsComplete)
        {
            MaxMediationManager.intertistial.ShowAds(onWatchAdsComplete);
        }
        public void ShowAds(int id)
        {
            var onWatchAdsFinished = events[id];
            MaxMediationManager.rewarded.ShowAds(() =>
            {
                onWatchAdsFinished?.Invoke();
            });
        }
    }
}