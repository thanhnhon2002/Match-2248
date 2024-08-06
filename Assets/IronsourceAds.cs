using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IronsourceAds : MonoBehaviour
{

    //public static IronsourceAds Instance;
    //public bool available { get; private set; }

    //private void Awake()
    //{
    //    if(Instance == null)
    //    {
    //        Instance = this;
    //        DontDestroyOnLoad(gameObject);
    //        return;
    //    }
    //    else Destroy(gameObject);
    //    IronSource.Agent.init();
    //}

    //private void Start()
    //{
    //    //Add AdInfo Banner Events
    //    IronSourceBannerEvents.onAdLoadedEvent += BannerOnAdLoadedEvent;
    //    IronSourceBannerEvents.onAdLoadFailedEvent += BannerOnAdLoadFailedEvent;
    //    IronSourceBannerEvents.onAdClickedEvent += BannerOnAdClickedEvent;
    //    IronSourceBannerEvents.onAdScreenPresentedEvent += BannerOnAdScreenPresentedEvent;
    //    IronSourceBannerEvents.onAdScreenDismissedEvent += BannerOnAdScreenDismissedEvent;
    //    IronSourceBannerEvents.onAdLeftApplicationEvent += BannerOnAdLeftApplicationEvent;
    //}

    //public void LoadBanner()
    //{
    //    Debug.Log("Load MREC");
    //    IronSource.Agent.loadBanner(IronSourceBannerSize.RECTANGLE, IronSourceBannerPosition.BOTTOM);
    //}

    //public void ShowAds()
    //{
    //    if (GameSystem.userdata.boughtItems.Contains(IAP_ID.no_ads.ToString())) return;
    //    if (!available) LoadBanner();
    //    IronSource.Agent.displayBanner();
    //}

    //public void HideAds()
    //{
    //    IronSource.Agent.displayBanner();
    //}

    ///************* Banner AdInfo Delegates *************/
    ////Invoked once the banner has loaded
    //void BannerOnAdLoadedEvent(IronSourceAdInfo adInfo)
    //{
    //    Debug.Log("MREC Loaded");
    //    available = true;
    //}
    ////Invoked when the banner loading process has failed.
    //void BannerOnAdLoadFailedEvent(IronSourceError ironSourceError)
    //{
    //    Debug.Log("MREC Load fail " + ironSourceError.getDescription());
    //    available = false;
    //}
    //// Invoked when end user clicks on the banner ad
    //void BannerOnAdClickedEvent(IronSourceAdInfo adInfo)
    //{
    //}
    ////Notifies the presentation of a full screen content following user click
    //void BannerOnAdScreenPresentedEvent(IronSourceAdInfo adInfo)
    //{
    //}
    ////Notifies the presented screen has been dismissed
    //void BannerOnAdScreenDismissedEvent(IronSourceAdInfo adInfo)
    //{
    //    LoadBanner();
    //}
    ////Invoked when the user leaves the app
    //void BannerOnAdLeftApplicationEvent(IronSourceAdInfo adInfo)
    //{
    //}
}
