using GoogleMobileAds.Api;
using GoogleMobileAds.Ump.Api;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AdmobManager : MonoBehaviour
{
    public static AdmobManager Instance;
    public static bool isReady = false;
    [SerializeField] private bool showDebug;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        ConsentRequestParameters request = new ConsentRequestParameters();
        ConsentInformation.Update(request, OnConsentInfoUpdated);
    }

    public void Init()
    {
        MobileAds.Initialize(initStatus =>
        {
            if (showDebug) Debug.Log("init finish with status = " + initStatus);
            isReady = true;
            var ads = GetComponentsInChildren<AdmobAds>();
            for (int i = 0; i < ads.Length; i++)
            {
                ads[i].Init();
                ads[i].LoadAds();
            }
        });
    }
    void OnConsentInfoUpdated(FormError consentError)
    {
        if (consentError != null)
        {
            Debug.LogError(consentError);
            return;
        }
        ConsentForm.LoadAndShowConsentFormIfRequired((FormError formError) =>
        {
            if (formError != null)
            {
                UnityEngine.Debug.LogError(consentError);
                return;
            }
            if (ConsentInformation.CanRequestAds())
            {
                Init();
            }
        });
    }
}