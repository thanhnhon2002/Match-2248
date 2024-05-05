using System;
using UnityEngine;

namespace DarkcupGames
{
    public abstract class MaxMediationAds : MonoBehaviour
    {
        [SerializeField] protected bool showDebug = false;
        public abstract void Init();
        public abstract void LoadAds();
        public abstract void ShowAds(Action onShowAdsComplete);
        public abstract bool IsAdAvailable();
        public abstract bool IsShowingAds();
    }
}