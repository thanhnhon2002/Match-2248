using System.Collections;
using UnityEngine;

namespace DarkcupGames
{
    public class MaxMediationManager : MonoBehaviour
    {
        public static MaxMediationManager instance;
        public string SDK_KEY;

        public static MaxMediationBanner banner { get; private set; }
        public static MaxMediationIntertistial intertistial { get; private set; }
        public static MaxMediationReward rewarded { get; private set; }

        private MaxMediationAds[] ads;
        private bool ready;

        private void Awake()
        {
            if (instance == null)
            {
                instance = this;
                DontDestroyOnLoad(gameObject);
            } else
            {
                Destroy(gameObject);
            }
        }
        private void Start()
        {
            Init();
        }

        public void Init()
        {
            MaxSdkCallbacks.OnSdkInitializedEvent += (MaxSdkBase.SdkConfiguration sdkConfiguration) =>
            {
                ready = true;
                ads = GetComponentsInChildren<MaxMediationAds>();
                for (int i = 0; i < ads.Length; i++)
                {
                    if (ads[i] is MaxMediationBanner) banner = (MaxMediationBanner)ads[i];
                    if (ads[i] is MaxMediationIntertistial) intertistial = (MaxMediationIntertistial)ads[i];
                    if (ads[i] is MaxMediationReward) rewarded = (MaxMediationReward)ads[i];

                    ads[i].Init();
                    ads[i].LoadAds();
                }
            };
            MaxSdk.SetSdkKey(SDK_KEY);
            MaxSdk.SetUserId("USER_ID");
            MaxSdk.InitializeSdk();
        }
    }
}