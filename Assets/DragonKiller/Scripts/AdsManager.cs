using UnityEngine;
using System.Collections;
using System;
using Rezero;

#if GOOGLE_MOBILE_ADS
using GoogleMobileAds;
using GoogleMobileAds.Api;
#endif

#if UNITY_ADS
using UnityEngine.Advertisements;
#endif

namespace Rezero
{
    public class AdsManager : Singleton<AdsManager>
    {
#if GOOGLE_MOBILE_ADS
        BannerView bannerView;
        InterstitialAd interstitial;
        AdRequest requestBanner;
        AdRequest requestInterstitial;
        RewardBasedVideoAd rewardBasedVideo;
#endif

#if GOOGLE_MOBILE_ADS
        public string AdmobAppID = "ca-app-pub-5173228928149464~2558832548";
        public string AdmobBannerIdANDROID = "ca-app-pub-5173228928149464/2711532327";
        public string AdmobInterstitialIdANDROID = "ca-app-pub-5173228928149464/3101704127";
#endif
#if UNITY_ADS
        public string UnityAndroidGameID = "1633868";
#endif

        public bool basedTimeInterstitialAtGameOver = false;
        public int numberOfPlayToShowInterstitial = 5;
        public float numberOfMinutesToShowAnInterstitialAtGameOver = 2;
        // Interstitials will use Unity Ads Video if its ready instead Admob
        public bool UseUnityAdsVideoInterstitial = false;
        public bool NO_ADS = false;

        float realTimeSinceStartup;

        void Awake()
        {
            instance = this;
            Set();
        }

        void Start()
        {

        }

        void Update()
        {

        }

        public void Set()
        {
            realTimeSinceStartup = Time.realtimeSinceStartup;
#if UNITY_ANDROID && GOOGLE_MOBILE_ADS
            MobileAds.Initialize(AdmobAppID);
#else
            string appId = "unexpected_platform";
#endif

#if UNITY_ADS
            Advertisement.Initialize(UnityAndroidGameID);
#endif


#if GOOGLE_MOBILE_ADS
            bannerView = new BannerView(AdmobBannerIdANDROID, AdSize.SmartBanner, AdPosition.Bottom);
            requestBanner = new AdRequest.Builder().Build();
            interstitial = new InterstitialAd(AdmobInterstitialIdANDROID);
            this.rewardBasedVideo = RewardBasedVideoAd.Instance;
#endif
            RequestInterstitial();
        }

        void RequestInterstitial()
        {
#if GOOGLE_MOBILE_ADS
            requestInterstitial = new AdRequest.Builder().Build();
            interstitial.LoadAd(requestInterstitial);
#endif
        }

#if GOOGLE_MOBILE_ADS
        // Called when an ad request has successfully loaded.
        void HandleAdLoaded(object sender, EventArgs e)
        {

        }
        // Called when an ad request failed to load.
        void HandleAdFailedToLoad(object sender, EventArgs e)
        {
            Invoke("ShowBanner", 10);
        }
        // Called when an ad is clicked.
        void HandleAdOpened(object sender, EventArgs e)
        {

        }
        // Called when the user is about to return to the app after an ad click.
        void HandleAdClosing(object sender, EventArgs e)
        {

        }
        // Called when the user returned from the app after an ad click.
        void HandleAdClosed(object sender, EventArgs e)
        {

        }
        // Called when the ad click caused the user to leave the application.
        void HandleAdLeftApplication(object sender, EventArgs e)
        {

        }
#endif
        public void ShowBanner()
        {
            if (NO_ADS)
                return;
#if GOOGLE_MOBILE_ADS
            bannerView.LoadAd(requestBanner);
            bannerView.Show();
            bannerView.OnAdLoaded -= HandleAdLoaded;
            bannerView.OnAdFailedToLoad -= HandleAdFailedToLoad;
            bannerView.OnAdOpening -= HandleAdOpened;
            bannerView.OnAdClosed -= HandleAdClosed;

            // Called when an ad request has successfully loaded.
            bannerView.OnAdLoaded += HandleAdLoaded;
            // Called when an ad request failed to load.
            bannerView.OnAdFailedToLoad += HandleAdFailedToLoad;
            // Called when an ad is clicked.
            bannerView.OnAdOpening += HandleAdOpened;
            // Called when the user returned from the app after an ad click.
            bannerView.OnAdClosed += HandleAdClosed;
#endif
        }

        public void ShowAdsGameOver()
        {
            if (NO_ADS)
                return;
            bool showAds = false;

            if (basedTimeInterstitialAtGameOver)
            {
                float t = Time.realtimeSinceStartup;

                float ourTIme = numberOfMinutesToShowAnInterstitialAtGameOver * 60;
                if ((t - realTimeSinceStartup) > ourTIme)
                {
                    _ShowInterstitial();
                    realTimeSinceStartup = t;
                }
            }
            else
            {
                int count = PlayerPrefs.GetInt("numberOfPlayToShowInterstitial", 0);

                showAds = count >= numberOfPlayToShowInterstitial;

                if (showAds)
                {
                    PlayerPrefs.SetInt("numberOfPlayToShowInterstitial", 0);
                    PlayerPrefs.Save();
                    _ShowInterstitial();
                }
                else
                {
                    PlayerPrefs.SetInt("numberOfPlayToShowInterstitial", count);
                    PlayerPrefs.Save();
                }

            }
        }

        private void _ShowInterstitial()
        {
            if (NO_ADS)
                return;
            if (UseUnityAdsVideoInterstitial)
            {
#if UNITY_ADS
                if(Advertisement.IsReady())
                {
                    Advertisement.Show();
                }
                else
                {
#if GOOGLE_MOBILE_ADS
                    if (interstitial.IsLoaded())
                    {
                        interstitial.Show();
                    }
                    else
                    {
                        RequestInterstitial();
                    }
#endif
                }
#endif
            }
            else
            {
#if GOOGLE_MOBILE_ADS
                if (interstitial.IsLoaded())
                {
                    interstitial.Show();
                }
                else
                {
                    RequestInterstitial();
                }
#endif
            }
        }

        public void Show_Banner()
        {
#if GOOGLE_MOBILE_ADS
            if (bannerView != null)
            {
                Debug.Log("Show current banner");
                bannerView.Show();
            }
#endif
        }

        public void Hide_Banner()
        {
#if GOOGLE_MOBILE_ADS
            if (bannerView != null)
            {
                Debug.Log("Hide current banner");
                bannerView.Hide();
            }
#endif
        }

        public void ShowRewardedAd()
        {
#if UNITY_ADS
            if (Advertisement.IsReady("rewardedVideo"))
            {
                ShowOptions options = new ShowOptions();
                options.resultCallback = HandleShowResult;

                Advertisement.Show("rewardedVideo", options);
            }
#endif
        }

#if UNITY_ADS
        private void HandleShowResult(ShowResult result)
        {
            switch (result)
            {
                case ShowResult.Finished:
                    Debug.Log("The ad was successfully shown.");
                    GUIController.Instance.RevivePlayer();
                    break;
                case ShowResult.Skipped:
                    Debug.Log("The ad was skipped before reaching the end.");
                    break;
                case ShowResult.Failed:
                    Debug.LogError("The ad failed to be shown.");
                    break;
            }
        }
#endif
    }
}