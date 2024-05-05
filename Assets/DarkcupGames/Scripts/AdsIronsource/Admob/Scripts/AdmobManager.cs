using GoogleMobileAds.Api;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AdmobManager : MonoBehaviour
{
    public static AdmobManager Instance;
    [SerializeField] private bool showDebug;

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        Init();
    }

    public void Init()
    {
        MobileAds.Initialize(initStatus =>
        {
            if (showDebug) Debug.Log("init finish with status = " + initStatus);
            var ads = GetComponentsInChildren<AdmobAds>();
            for (int i = 0; i < ads.Length; i++)
            {
                ads[i].Init();
                ads[i].LoadAds();
            }
        });
    }
}