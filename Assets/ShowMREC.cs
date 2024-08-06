using GoogleMobileAds.Api;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShowMREC : MonoBehaviour
{
    private AdmobMREC mREC;

    private void Awake()
    {
        mREC = FindObjectOfType<AdmobMREC>();
    }

    private void OnEnable()
    {
        mREC.ShowAds(null);
        mREC.bannerView.SetPosition(AdPosition.Top);
    }

    private void OnDisable()
    {
        mREC.HideAds();
    }
}
