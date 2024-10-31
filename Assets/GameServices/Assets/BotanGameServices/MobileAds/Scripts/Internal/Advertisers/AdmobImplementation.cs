#if BOTANGAMESERVICES_ADMOB
namespace BotanGameServices.AdsPackage.Local
{
    using UnityEngine.Events;
    using UnityEngine;
    using GoogleMobileAds.Api;
    using System.Collections.Generic;
    using System.Collections;
    using GoogleMobileAds.Ump.Api;
    using System;

    public class AdmobImplementation : MonoBehaviour, IAdProvider
    {
        private const float reloadTime = 30;
        private readonly int maxRetryCount = 10;

        private UnityAction<bool> onRewardedVideoClosed;
        private UnityAction<bool> onRewardedInterstitialClosed;

        private UnityAction onInterstitialClosed;
        private UnityAction onAppOpenClosed;
        private UnityAction onConsentPopupClosed;
        private UnityAction onInitialized;
        private InterstitialAd interstitial;
        private DateTime appOpenExpireTime;
        private AppOpenAd appOpen;
        private ConsentForm _consentForm;
        private BannerView banner;
        private RewardedAd rewardedVideo;
        private RewardedInterstitialAd rewardedInterstitial;
        private Events events;
        private BannerPosition position;
        private BannerType bannerType;
        private DebugGeography debugGeography;
        private string rewardedVideoId;
        private string interstitialId;
        private string appOpenId;
        private string rewardedInterstitialId;
        private string bannerId;
        private string mrecId;
        private string designedForFamilies;
        private string testDeviceID;
        private int currentRetryRewardedVideo;
        private int currentRetryRewardedInterstitial;
        private int currentRetryInterstitial;
        private int currentRetryAppOpen;
        private bool initialized;
        private bool rewardedVideoWatched;
        private bool rewardedInterstitialWatched;
        private bool bannerLoaded;
        private bool interstitialFailedToLoad;
        private bool rewardedVideoFailedToLoad;
        private bool rewardedInterstitialFailedToLoad;
        private bool appOpenFailedToLoad;
        private bool directedForChildren;



        #region Initialize
        #region InterfaceImplementation
        public void SetDirectedForChildren(bool active)
        {
            directedForChildren = active;
        }

        /// <summary>
        /// Initializing Admob
        /// </summary>
        /// <param name="consent">user consent -> if true show personalized ads</param>
        /// <param name="platformSettings">contains all required settings for this publisher</param>
        public void InitializeAds(PlatformSettings platformSettings, UnityAction onInitialized)
        {
            if (initialized == false)
            {
                BotanGameServicesLogger.AddLog("Start Initialization");
                events = new Events();

                this.onInitialized = onInitialized;
                //get settings
                PlatformSettings settings = platformSettings;

                if (settings.testMode)
                {
#if UNITY_ANDROID
                    bannerId = "ca-app-pub-3940256099942544/6300978111";
                    mrecId = bannerId;
                    interstitialId = "ca-app-pub-3940256099942544/1033173712";
                    rewardedVideoId = "ca-app-pub-3940256099942544/5224354917";
                    rewardedInterstitialId = "ca-app-pub-3940256099942544/5354046379";
                    appOpenId = "ca-app-pub-3940256099942544/9257395921";
#else
                    bannerId = "ca-app-pub-3940256099942544/2934735716";
                    mrecId = bannerId;
                    interstitialId = "ca-app-pub-3940256099942544/4411468910";
                    rewardedVideoId = "ca-app-pub-3940256099942544/1712485313";
                    rewardedInterstitialId = "ca-app-pub-3940256099942544/6978759866";
                    appOpenId = "ca-app-pub-3940256099942544/5575463023";
#endif
                }
                else
                {
                    //apply settings
                    interstitialId = settings.idInterstitial.id;
                    bannerId = settings.idBanner.id;
                    mrecId = settings.idMRec.id;
                    rewardedVideoId = settings.idRewarded.id;
                    rewardedInterstitialId = settings.idRewardedInterstitial.id;
                    appOpenId = settings.idOpenApp.id;
                }
                TagForChildDirectedTreatment tagFororChildren;
                TagForUnderAgeOfConsent tagForUnderAge;
                MaxAdContentRating contentRating;

                if (directedForChildren == true)
                {
                    designedForFamilies = "true";
                    tagFororChildren = TagForChildDirectedTreatment.True;
                    tagForUnderAge = TagForUnderAgeOfConsent.True;
                    contentRating = MaxAdContentRating.G;
                }
                else
                {
                    designedForFamilies = "false";
                    tagFororChildren = TagForChildDirectedTreatment.Unspecified;
                    tagForUnderAge = TagForUnderAgeOfConsent.Unspecified;
                    contentRating = MaxAdContentRating.Unspecified;
                }


                RequestConfiguration requestConfiguration = new RequestConfiguration
                {
                    TagForChildDirectedTreatment = tagFororChildren,
                    MaxAdContentRating = contentRating,
                    TagForUnderAgeOfConsent = tagForUnderAge,
                    TestDeviceIds = new List<string> { settings.testDevice }
                };

                if (!string.IsNullOrEmpty(testDeviceID))
                {
                    requestConfiguration.TestDeviceIds.Add(settings.testDevice);
                }

                MobileAds.SetRequestConfiguration(requestConfiguration);

                //verify settings

                BotanGameServicesLogger.AddLog($"Test mode: " + settings.testMode);
                BotanGameServicesLogger.AddLog($"{settings.appId.displayName} : {settings.appId.id}");
                BotanGameServicesLogger.AddLog($"{settings.idBanner.displayName} : {bannerId}");
                BotanGameServicesLogger.AddLog($"{settings.idMRec.displayName} : {mrecId}");
                BotanGameServicesLogger.AddLog($"{settings.idInterstitial.displayName} : {interstitialId}");
                BotanGameServicesLogger.AddLog($"{settings.idRewarded.displayName} : {rewardedVideoId}");
                BotanGameServicesLogger.AddLog($"{settings.idRewardedInterstitial.displayName} : {rewardedInterstitialId}");
                BotanGameServicesLogger.AddLog($"{settings.idOpenApp.displayName} : {appOpenId}");
                BotanGameServicesLogger.AddLog($"Directed for children: {directedForChildren}");

                //preparing Admob SDK for initialization

                MobileAds.RaiseAdEventsOnUnityMainThread = true;
                MobileAds.SetiOSAppPauseOnBackground(true);

                MobileAds.Initialize(InitComplete);
            }
        }
        #endregion


        private void InitComplete(InitializationStatus status)
        {
            BotanGameServicesLogger.AddLog("Initialization complete: ");
            Dictionary<string, AdapterStatus> adapterState = status.getAdapterStatusMap();
            BotanGameServicesLogger.AddLog("Adapter status: ");
            foreach (var adapter in adapterState)
            {
                BotanGameServicesLogger.AddLog(adapter.Key + " " + adapter.Value.InitializationState + " " + adapter.Value.Description);
            }
            if (!string.IsNullOrEmpty(rewardedVideoId))
            {
                LoadRewardedVideo();
            }
            if (!string.IsNullOrEmpty(interstitialId))
            {
                LoadInterstitial();
            }
            if (!string.IsNullOrEmpty(rewardedInterstitialId))
            {
                LoadRewardedInterstitial();
            }

            if (!string.IsNullOrEmpty(appOpenId))
            {
                LoadAppOpen();
            }

            initialized = true;

            onInitialized?.Invoke();
        }
        #endregion


        #region Banner
        #region InterfaceImplementation
        /// <summary>
        /// Show Admob banner
        /// </summary>
        /// <param name="position"> can be TOP or BOTTOM</param>
        ///  /// <param name="bannerType"> can be Banner or SmartBanner</param>
        public void ShowBanner(BannerPosition position, BannerType bannerType, Vector2Int customSize, Vector2Int customPosition)
        {
            ShowBanner(bannerId, position, bannerType, customSize, customPosition, false);
        }


        /// <summary>
        /// Hides Admob banner
        /// </summary>
        public void HideBanner()
        {
            BotanGameServicesLogger.AddLog("Hide banner");

            if (banner != null)
            {
                if (bannerLoaded == false)
                {
                    DestroyBanner();
                }
                else
                {
                    //hide the banner -> will be available later without loading
                    banner.Hide();
                }
            }
        }

        #endregion

        public void ShowBanner(string bannerID, BannerPosition position, BannerType bannerType, Vector2Int customSize, Vector2Int customPosition, bool collapsable)
        {
            bannerLoaded = false;
            //this.DisplayResult = DisplayResult;
            if (banner != null)
            {
                if (this.position == position && this.bannerType == bannerType)
                {
                    BotanGameServicesLogger.AddLog("Show Banner");
                    bannerLoaded = true;
                    banner.Show();
                }
                else
                {
                    LoadBanner(bannerID, position, bannerType, customSize, customPosition, collapsable);
                }
            }
            else
            {
                LoadBanner(bannerID, position, bannerType, customSize, customPosition, collapsable);
            }
        }
        /// <summary>
        /// Loads an Admob banner
        /// </summary>
        /// <param name="position">display position</param>
        /// <param name="bannerType">can be normal banner or smart banner</param>
        private void LoadBanner(string bannerID, BannerPosition position, BannerType bannerType, Vector2Int customSize, Vector2Int customPosition, bool collapsable)
        {
            BotanGameServicesLogger.AddLog("Start Loading Banner");
            //setup banner

            DestroyBanner();

            this.position = position;
            this.bannerType = bannerType;

            AdSize adSize;
            switch (bannerType)
            {
                case BannerType.Banner:
                    adSize = AdSize.Banner;
                    break;
                case BannerType.IABBanner:
                    adSize = AdSize.IABBanner;
                    break;
                case BannerType.Leaderboard:
                    adSize = AdSize.Leaderboard;
                    break;
                case BannerType.MediumRectangle:
                    adSize = AdSize.MediumRectangle;
                    break;
                case BannerType.Adaptive:
                    adSize = AdSize.GetCurrentOrientationAnchoredAdaptiveBannerAdSizeWithWidth(AdSize.FullWidth);
                    break;
                case BannerType.Custom:
                    adSize = new AdSize(customSize.x, customSize.y);
                    break;
                default:
                    BotanGameServicesLogger.AddLog($"Banner Type: {bannerType} not supported by Admob, BannerType.Banner will be used");
                    adSize = AdSize.Banner;
                    break;
            }

            AdPosition adPosition = AdPosition.Top;
            // create an instance of a banner view first.
            switch (position)
            {
                case BannerPosition.Bottom:
                    adPosition = AdPosition.Bottom;
                    break;
                case BannerPosition.BottomLeft:
                    adPosition = AdPosition.BottomLeft;
                    break;
                case BannerPosition.BottomRight:
                    adPosition = AdPosition.BottomRight;
                    break;
                case BannerPosition.Center:
                    adPosition = AdPosition.Center;
                    break;
                case BannerPosition.Custom:
                    break;
                case BannerPosition.Top:
                    adPosition = AdPosition.Top;
                    break;
                case BannerPosition.TopLeft:
                    adPosition = AdPosition.TopLeft;
                    break;
                case BannerPosition.TopRight:
                    adPosition = AdPosition.TopRight;
                    break;
                default:
                    BotanGameServicesLogger.AddLog($"Banner Position: {position} not supported by Admob, BannerPosition.Top will be used");
                    break;
            }

            if (position == BannerPosition.Custom)
            {
                banner = new BannerView(bannerID, adSize, customPosition.x, customPosition.y);
            }
            else
            {
                banner = new BannerView(bannerID, adSize, adPosition);
            }

            // listen to events the banner may raise.
            banner.OnBannerAdLoaded += BannerLoadSucces;
            banner.OnBannerAdLoadFailed += BannerLoadFailed;
            banner.OnAdPaid += BannerAdPaied;
            banner.OnAdImpressionRecorded += BannerImpressionRecorded;
            banner.OnAdClicked += BannerClicked;
            banner.OnAdFullScreenContentOpened += BannerFullScreenOpened;
            banner.OnAdFullScreenContentClosed += BannerFullScreenClose;

            //request and load banner
            var request = CreateRequest();
            if (collapsable)
            {

                if (position.ToString().Contains("Top"))
                {
                    BotanGameServicesLogger.AddLog("Collapsible banner top");
                    request.Extras.Add("collapsible", "top");
                }
                else
                {
                    BotanGameServicesLogger.AddLog("Collapsible banner bottom");
                    request.Extras.Add("collapsible", "bottom");
                }
                request.Extras.Add("collapsible_request_id", Guid.NewGuid().ToString());
            }

            banner.LoadAd(request);
        }

        private void DestroyBanner()
        {
            if (banner != null)
            {
                banner.OnBannerAdLoaded -= BannerLoadSucces;
                banner.OnBannerAdLoadFailed -= BannerLoadFailed;
                banner.OnAdPaid -= BannerAdPaied;
                banner.OnAdImpressionRecorded -= BannerImpressionRecorded;
                banner.OnAdClicked -= BannerClicked;
                banner.OnAdFullScreenContentOpened -= BannerFullScreenOpened;
                banner.OnAdFullScreenContentClosed -= BannerFullScreenClose;
                banner.Destroy();
            }
            banner = null;
        }

        /// <summary>
        /// Admob specific event triggered after banner was loaded into the banner view.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BannerLoadSucces()
        {
            BotanGameServicesLogger.AddLog("Banner Loaded");
            bannerLoaded = true;
            events.TriggerBannerLoadSucces();
        }


        /// <summary>
        /// Admob specific event triggered after banner failed to load
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="loadAdError"></param>
        private void BannerLoadFailed(LoadAdError loadAdError)
        {
            BotanGameServicesLogger.AddLog("Admob Banner Failed To Load ");
            LogErrorMessage(loadAdError);
            DestroyBanner();
            bannerLoaded = false;
            events.TriggerBannerLoadFailed(loadAdError.ToString());
        }


        /// <summary>
        /// Raised when the ad is estimated to have earned money.
        /// </summary>
        /// <param name="adValue"></param>
        private void BannerAdPaied(AdValue adValue)
        {
            BotanGameServicesLogger.AddLog($"Banner view paid {adValue.Value} {adValue.CurrencyCode}.");
        }


        /// <summary>
        /// Raised when an impression is recorded for an ad.
        /// </summary>
        private void BannerImpressionRecorded()
        {
            BotanGameServicesLogger.AddLog("Banner view recorded an impression.");
        }


        /// <summary>
        /// Raised when a click is recorded for an ad.
        /// </summary>
        private void BannerClicked()
        {
            BotanGameServicesLogger.AddLog("Banner view was clicked.");
            events.TriggerBannerClicked();
        }


        /// <summary>
        /// Raised when an ad opened full screen content.
        /// </summary>
        private void BannerFullScreenOpened()
        {
            BotanGameServicesLogger.AddLog("Banner view full screen content opened.");
        }


        /// <summary>
        /// Raised when the ad closed full screen content.
        /// </summary>
        private void BannerFullScreenClose()
        {
            BotanGameServicesLogger.AddLog("Banner view full screen content closed.");
        }
        #endregion


        #region Interstitial
        #region InterfaceImplementation
        /// <summary>
        /// Check if Admob interstitial is available
        /// </summary>
        /// <returns>true if an interstitial is available</returns>
        public bool IsInterstitialAvailable()
        {
            if (interstitial != null)
            {
                return interstitial.CanShowAd();
            }
            return false;
        }


        /// <summary>
        /// Show Admob interstitial
        /// </summary>
        /// <param name="InterstitialClosed">callback called when user closes interstitial </param>
        public void ShowInterstitial(UnityAction InterstitialClosed)
        {
            if (IsInterstitialAvailable())
            {
                onInterstitialClosed = InterstitialClosed;
                interstitial.Show();
            }
            else
            {
                BotanGameServicesLogger.AddLog("Interstitial ad cannot be shown.");
            }
        }
        #endregion


        /// <summary>
        /// Loads an Admob interstitial
        /// </summary>
        private void LoadInterstitial()
        {
            BotanGameServicesLogger.AddLog("Start Loading Interstitial");

            // Clean up the old ad before loading a new one.
            if (interstitial != null)
            {
                interstitial.OnAdPaid -= InterstitialAdPaied;
                interstitial.OnAdImpressionRecorded -= ImpressionRecorded;
                interstitial.OnAdClicked -= AdClicked;
                interstitial.OnAdFullScreenContentOpened -= AdFullScreenOpened;
                interstitial.OnAdFullScreenContentClosed -= AdFullScreenClosed;
                interstitial.OnAdFullScreenContentFailed -= AdFullScreenFailed;
                interstitial.Destroy();
                interstitial = null;
            }

            // create our request used to load the ad.
            var adRequest = CreateRequest();

            // Load an interstitial ad
            InterstitialAd.Load(interstitialId, adRequest, InterstitialLoadCallback);
        }


        private void InterstitialLoadCallback(InterstitialAd ad, LoadAdError loadAdError)
        {
            // if error is not null, the load request failed.
            if (loadAdError != null || ad == null)
            {
                InterstitialFailed(loadAdError);
                return;
            }
            else
            {
                InterstitialLoaded(ad);
            }
        }


        /// <summary>
        /// Admob specific event triggered after an interstitial was loaded
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void InterstitialLoaded(InterstitialAd ad)
        {
            interstitial = ad;

            BotanGameServicesLogger.AddLog("Interstitial Loaded");

            interstitial.OnAdPaid += InterstitialAdPaied;
            interstitial.OnAdImpressionRecorded += ImpressionRecorded;
            interstitial.OnAdClicked += AdClicked;
            interstitial.OnAdFullScreenContentOpened += AdFullScreenOpened;
            interstitial.OnAdFullScreenContentClosed += AdFullScreenClosed;
            interstitial.OnAdFullScreenContentFailed += AdFullScreenFailed;

            currentRetryInterstitial = 0;
            events.TriggerInterstitialLoadSucces();
        }


        /// <summary>
        /// Admob specific event triggered if an interstitial failed to load
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void InterstitialFailed(LoadAdError e)
        {
            // Gets the domain from which the error came.
            //string domain = loadAdError.GetDomain();

            // Gets the error code. See
            // https://developers.google.com/android/reference/com/google/android/gms/ads/AdRequest
            // and https://developers.google.com/admob/ios/api/reference/Enums/GADErrorCode
            // for a list of possible codes.
            //int code = loadAdError.GetCode();

            // Gets an error message.
            // For example "Account not approved yet". See
            // https://support.google.com/admob/answer/9905175 for explanations of
            // common errors.
            //string message = loadAdError.GetMessage();

            // Gets the cause of the error, if available.
            //AdError underlyingError = loadAdError.GetCause();



            // Get response information, which may include results of mediation requests.
            ResponseInfo responseInfo = e.GetResponseInfo();


            BotanGameServicesLogger.AddLog("Interstitial Failed To Load ");
            BotanGameServicesLogger.AddLog($"Interstitial -> Load error string: {e}");
            BotanGameServicesLogger.AddLog($"Interstitial -> Response info: {responseInfo}");


            //try again to load a rewarded video
            if (currentRetryInterstitial < maxRetryCount)
            {
                currentRetryInterstitial++;
                BotanGameServicesLogger.AddLog("Retry " + currentRetryInterstitial);

                interstitialFailedToLoad = true;
            }
            events.TriggerInterstitialLoadFailed(e.ToString());
        }


        /// <summary>
        /// Admob specific event triggered after an interstitial was closed
        /// </summary>
        private void InterstitialClosed()
        {
            BotanGameServicesLogger.AddLog("Reload Interstitial");

            //trigger complete event
            StartCoroutine(CompleteMethodInterstitial());

            //reload interstitial
            LoadInterstitial();
        }


        /// <summary>
        ///  Because Admob has some problems when used in multi threading applications with Unity a frame needs to be skipped before returning to application
        /// </summary>
        /// <returns></returns>
        private IEnumerator CompleteMethodInterstitial()
        {
            yield return null;
            if (onInterstitialClosed != null)
            {
                onInterstitialClosed();
                onInterstitialClosed = null;
            }
        }


        /// <summary>
        /// Raised when the ad is estimated to have earned money.
        /// </summary>
        /// <param name="adValue"></param>
        private void InterstitialAdPaied(AdValue adValue)
        {
            BotanGameServicesLogger.AddLog($"Interstitial ad paid {adValue.Value} {adValue.CurrencyCode}");
        }


        /// <summary>
        /// Raised when an impression is recorded for an ad.
        /// </summary>        
        private void ImpressionRecorded()
        {
            BotanGameServicesLogger.AddLog("Interstitial ad recorded an impression.");
        }


        /// <summary>
        ///Raised when a click is recorded for an ad. 
        /// </summary>
        private void AdClicked()
        {
            BotanGameServicesLogger.AddLog("Interstitial ad was clicked.");
            events.TriggerInterstitialClicked();
        }


        /// <summary>
        /// Raised when an ad opened full screen content.
        /// </summary>
        private void AdFullScreenOpened()
        {
            BotanGameServicesLogger.AddLog("Interstitial ad full screen content opened.");
        }


        /// <summary>
        ///Raised when the ad closed full screen content. 
        /// </summary>
        private void AdFullScreenClosed()
        {
            BotanGameServicesLogger.AddLog("Interstitial ad full screen content closed.");
            InterstitialClosed();
        }


        /// <summary>
        ///Raised when the ad failed to open full screen content. 
        /// </summary>
        /// <param name="error"></param>
        private void AdFullScreenFailed(AdError error)
        {
            BotanGameServicesLogger.AddLog($"Interstitial ad failed to open full screen content. Error: {error}");
            InterstitialClosed();
        }
        #endregion


        #region AppOpen
        #region InterfaceImplementation
        public bool IsAppOpenAvailable()
        {
            if (appOpen != null)
            {
                return appOpen.CanShowAd() && DateTime.Now < appOpenExpireTime;
            }
            return false;
        }

        public void ShowAppOpen(UnityAction appOpenClosed = null)
        {
            if (IsAppOpenAvailable())
            {
                onAppOpenClosed = appOpenClosed;
                appOpen.Show();
            }
            else
            {
                BotanGameServicesLogger.AddLog("AppOpen ad cannot be shown.");
                if (initialized == true)
                {
                    if (DateTime.Now > appOpenExpireTime)
                    {
                        LoadAppOpen();
                    }
                }
            }
        }
        #endregion

        private void LoadAppOpen()
        {
            BotanGameServicesLogger.AddLog("Start Loading App Open");

            // Clean up the old ad before loading a new one.
            if (appOpen != null)
            {
                appOpen.OnAdPaid -= AppOpenAdPaied;
                appOpen.OnAdImpressionRecorded -= AppOpenImpressionRecorded;
                appOpen.OnAdClicked -= AppOpenAdClicked;
                appOpen.OnAdFullScreenContentOpened -= AppOpenAdFullScreenOpened;
                appOpen.OnAdFullScreenContentClosed -= AppOpenAdFullScreenClosed;
                appOpen.OnAdFullScreenContentFailed -= AppOpenAdFullScreenFailed;
                appOpen.Destroy();
                appOpen = null;
            }

            // create our request used to load the ad.
            var adRequest = CreateRequest();

            // Load an interstitial ad
            AppOpenAd.Load(appOpenId, adRequest, AppOpenLoadCallback);
        }

        private void AppOpenAdFullScreenFailed(AdError error)
        {
            BotanGameServicesLogger.AddLog($"App open ad failed to open full screen content. Error: {error}");
            AppOpenClosed();
        }

        private void AppOpenAdFullScreenClosed()
        {
            BotanGameServicesLogger.AddLog("App open ad full screen content closed.");
            AppOpenClosed();
        }

        private void AppOpenClosed()
        {
            BotanGameServicesLogger.AddLog("Reload App Open");

            //trigger complete event
            StartCoroutine(CompleteMethodAppOpen());

            //reload app open
            LoadAppOpen();
        }

        IEnumerator CompleteMethodAppOpen()
        {
            yield return null;
            if (onAppOpenClosed != null)
            {
                onAppOpenClosed();
                onAppOpenClosed = null;
            }
        }

        private void AppOpenAdFullScreenOpened()
        {
            BotanGameServicesLogger.AddLog("Open app ad full screen content opened.");
        }

        private void AppOpenAdClicked()
        {
            BotanGameServicesLogger.AddLog("Open app ad was clicked.");
            events.TriggerAppOpenClicked();
        }

        private void AppOpenImpressionRecorded()
        {
            BotanGameServicesLogger.AddLog("Open app ad recorded an impression.");
        }

        private void AppOpenAdPaied(AdValue adValue)
        {
            BotanGameServicesLogger.AddLog($"Open app ad paid {adValue.Value} {adValue.CurrencyCode}");
        }

        private void AppOpenLoadCallback(AppOpenAd ad, LoadAdError loadAdError)
        {
            // if error is not null, the load request failed.
            if (loadAdError != null || ad == null)
            {
                AppOpenFailed(loadAdError);
                return;
            }
            else
            {
                AppOpenLoaded(ad);
            };
        }

        private void AppOpenLoaded(AppOpenAd ad)
        {
            appOpenExpireTime = DateTime.Now + TimeSpan.FromHours(4);
            appOpen = ad;

            BotanGameServicesLogger.AddLog("App Open Loaded");

            appOpen.OnAdPaid += AppOpenAdPaied;
            appOpen.OnAdImpressionRecorded += AppOpenImpressionRecorded;
            appOpen.OnAdClicked += AppOpenAdClicked;
            appOpen.OnAdFullScreenContentOpened += AppOpenAdFullScreenOpened;
            appOpen.OnAdFullScreenContentClosed += AppOpenAdFullScreenClosed;
            appOpen.OnAdFullScreenContentFailed += AppOpenAdFullScreenFailed;

            currentRetryAppOpen = 0;
            events.TriggerAppOpenLoadSucces();
        }

        private void AppOpenFailed(LoadAdError loadAdError)
        {
            BotanGameServicesLogger.AddLog($"App open ad failed to load error :{loadAdError}");

            if (currentRetryAppOpen < maxRetryCount)
            {
                currentRetryAppOpen++;
                BotanGameServicesLogger.AddLog("Retry " + currentRetryAppOpen);

                appOpenFailedToLoad = true;
            }
            events.TriggerAppOpenLoadFailed(loadAdError.ToString());
        }



        #endregion


        #region Rewarded
        #region InterfaceImplementation
        /// <summary>
        /// Check if Admob rewarded video is available
        /// </summary>
        /// <returns>true if a rewarded video is available</returns>
        public bool IsRewardedVideoAvailable()
        {
            if (rewardedVideo != null)
            {
                return rewardedVideo.CanShowAd();
            }
            return false;
        }


        /// <summary>
        /// Show Admob rewarded video
        /// </summary>
        /// <param name="CompleteMethod">callback called when user closes the rewarded video -> if true video was not skipped</param>
        public void ShowRewardedVideo(UnityAction<bool> CompleteMethod)
        {
            if (IsRewardedVideoAvailable())
            {
                onRewardedVideoClosed = CompleteMethod;
                rewardedVideoWatched = false;
                rewardedVideo.Show(RewardedVideoWatched);
            }
            else
            {
                BotanGameServicesLogger.AddLog("Rewarded video cannot be shown.");
            }
        }
        #endregion


        /// <summary>
        /// Loads an Admob rewarded video
        /// </summary>
        private void LoadRewardedVideo()
        {
            BotanGameServicesLogger.AddLog("Start Loading Rewarded Video");


            // Clean up the old ad before loading a new one.
            if (rewardedVideo != null)
            {
                rewardedVideo.OnAdPaid -= RewardedPaid;
                rewardedVideo.OnAdImpressionRecorded -= RewardedImpressionRec;
                rewardedVideo.OnAdClicked -= RewardedAdClicked;
                rewardedVideo.OnAdFullScreenContentOpened -= RewardedFullScreenOpened;
                rewardedVideo.OnAdFullScreenContentClosed -= RewardedFullScreenClosed;
                rewardedVideo.OnAdFullScreenContentFailed -= RewardedFullScreenFailed;
                rewardedVideo.Destroy();
                rewardedVideo = null;
            }

            // create our request used to load the ad.
            var adRequest = new AdRequest();

            // send the request to load the ad.
            RewardedAd.Load(rewardedVideoId, adRequest, LoadRewardedVideoCallback);
        }


        private void LoadRewardedVideoCallback(RewardedAd ad, LoadAdError loadAdError)
        {
            // if error is not null, the load request failed.
            if (loadAdError != null || ad == null)
            {
                RewardedVideoFailed(loadAdError);
                return;
            }
            else
            {
                RewardedVideoLoaded(ad);
            }
        }


        /// <summary>
        /// Admob specific event triggered after a rewarded video is loaded and ready to be watched
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void RewardedVideoLoaded(RewardedAd ad)
        {
            rewardedVideo = ad;
            BotanGameServicesLogger.AddLog("Rewarded Video Loaded.");

            rewardedVideo.OnAdPaid += RewardedPaid;
            rewardedVideo.OnAdImpressionRecorded += RewardedImpressionRec;
            rewardedVideo.OnAdClicked += RewardedAdClicked;
            rewardedVideo.OnAdFullScreenContentOpened += RewardedFullScreenOpened;
            rewardedVideo.OnAdFullScreenContentClosed += RewardedFullScreenClosed;
            rewardedVideo.OnAdFullScreenContentFailed += RewardedFullScreenFailed;

            currentRetryRewardedVideo = 0;
            events.TriggerRewardedVideoLoadSucces();
        }


        /// <summary>
        /// Admob specific event triggered if a rewarded video failed to load
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void RewardedVideoFailed(LoadAdError e)
        {
            LoadAdError loadAdError = e;

            // Gets the domain from which the error came.
            //string domain = loadAdError.GetDomain();

            // Gets the error code. See
            // https://developers.google.com/android/reference/com/google/android/gms/ads/AdRequest
            // and https://developers.google.com/admob/ios/api/reference/Enums/GADErrorCode
            // for a list of possible codes.
            //int code = loadAdError.GetCode();

            // Gets an error message.
            // For example "Account not approved yet". See
            // https://support.google.com/admob/answer/9905175 for explanations of
            // common errors.
            //string message = loadAdError.GetMessage();

            // Gets the cause of the error, if available.
            //AdError underlyingError = loadAdError.GetCause();


            // Get response information, which may include results of mediation requests.
            ResponseInfo responseInfo = loadAdError.GetResponseInfo();

            //BotanGameServicesLogger.AddLog($"Rewarded Video Failed: {loadAdError.}");
            //BotanGameServicesLogger.AddLog($"Rewarded Video Response info: {responseInfo}");


            //try again to load a rewarded video
            if (currentRetryRewardedVideo < maxRetryCount)
            {
                currentRetryRewardedVideo++;

                BotanGameServicesLogger.AddLog("Retry " + currentRetryRewardedVideo);

                rewardedVideoFailedToLoad = true;
            }
            events.TriggerRewardedVideoLoadFailed(e.ToString());
        }


        private void RewardedVideoWatched(Reward reward)
        {
            BotanGameServicesLogger.AddLog($"Rewarded Video Watched -> Reward amount: {reward.Amount} Reward type: {reward.Type}");
            rewardedVideoWatched = true;
#if UNITY_EDITOR
            RewardedAdClosed();
#endif
        }


        /// <summary>
        /// Admob specific event triggered when a rewarded video was skipped
        /// </summary>
        private void RewardedAdClosed()
        {
            BotanGameServicesLogger.AddLog("Rewarded Ad Closed");
            //trigger complete method
            StartCoroutine(CompleteMethodRewardedVideo(rewardedVideoWatched));
        }


        /// <summary>
        /// Because Admob has some problems when used in multi-threading applications with Unity a frame needs to be skipped before returning to application
        /// </summary>
        /// <returns></returns>
        private IEnumerator CompleteMethodRewardedVideo(bool val)
        {
            yield return null;
            if (onRewardedVideoClosed != null)
            {
                onRewardedVideoClosed(val);
                onRewardedVideoClosed = null;
            }

            //reload
            LoadRewardedVideo();
        }


        /// <summary>
        ///Raised when an impression is recorded for an ad.
        /// </summary>
        private void RewardedImpressionRec()
        {
            BotanGameServicesLogger.AddLog("Rewarded ad recorded an impression.");
        }


        /// <summary>
        /// Raised when the ad is estimated to have earned money.
        /// </summary>
        /// <param name="adValue"></param>
        private void RewardedPaid(AdValue adValue)
        {
            BotanGameServicesLogger.AddLog($"Rewarded ad paid {adValue.Value} {adValue.CurrencyCode}");
        }


        /// <summary>
        ///Raised when a click is recorded for an ad.
        /// </summary>
        private void RewardedAdClicked()
        {
            BotanGameServicesLogger.AddLog("Rewarded ad was clicked.");
            events.TriggerRewardedVideoClicked();
        }


        /// <summary>
        ///Raised when an ad opened full screen content. 
        /// </summary>
        private void RewardedFullScreenOpened()
        {
            BotanGameServicesLogger.AddLog("Rewarded ad full screen content opened.");
        }


        /// <summary>
        ///Raised when the ad closed full screen content.
        /// </summary>
        private void RewardedFullScreenClosed()
        {
            BotanGameServicesLogger.AddLog("Rewarded ad full screen content closed.");
#if !UNITY_EDITOR
            RewardedAdClosed();
#endif
        }


        /// <summary>
        ///Raised when the ad failed to open full screen content.
        /// </summary>
        /// <param name="error"></param>
        private void RewardedFullScreenFailed(AdError error)
        {
            BotanGameServicesLogger.AddLog($"Rewarded ad failed to open full screen content. Error: {error}");

            //Reload the rewarded ad so that we can show another one as soon as possible
            RewardedAdClosed();
        }
        #endregion


        #region RewardedInterstitial
        #region InterfaceImplementation
        public bool IsRewardedInterstitialAvailable()
        {
            if (rewardedInterstitial != null)
            {
                return rewardedInterstitial.CanShowAd();
            }
            return false;
        }

        public void ShowRewardedInterstitial(UnityAction<bool> completeMethod)
        {
            if (IsRewardedInterstitialAvailable())
            {
                onRewardedInterstitialClosed = completeMethod;
                rewardedInterstitialWatched = false;
                rewardedInterstitial.Show(RewardedInterstitialWatched);
            }
            else
            {
                BotanGameServicesLogger.AddLog("Rewarded interstitial cannot be shown.");
            }
        }
        #endregion
        private void RewardedInterstitialWatched(Reward reward)
        {
            BotanGameServicesLogger.AddLog($"Rewarded Interstitial Watched -> Reward amount: {reward.Amount} Reward type: {reward.Type}");
            rewardedInterstitialWatched = true;
#if UNITY_EDITOR
            RewardedInterstitialClosed();
#endif
        }

        private void RewardedInterstitialClosed()
        {
            BotanGameServicesLogger.AddLog("Rewarded Interstitial Closed");
            //trigger complete method
            StartCoroutine(CompleteMethodRewardedInterstitial(rewardedInterstitialWatched));
        }

        IEnumerator CompleteMethodRewardedInterstitial(bool rewardedVideoWatched)
        {
            yield return null;
            if (onRewardedInterstitialClosed != null)
            {
                onRewardedInterstitialClosed(rewardedVideoWatched);
                onRewardedInterstitialClosed = null;
            }

            //reload
            LoadRewardedInterstitial();
        }

        private void LoadRewardedInterstitial()
        {
            BotanGameServicesLogger.AddLog("Start Loading Rewarded Interstitial");


            // Clean up the old ad before loading a new one.
            if (rewardedInterstitial != null)
            {
                rewardedInterstitial.OnAdPaid -= RewardedInterstitialPaid;
                rewardedInterstitial.OnAdImpressionRecorded -= RewardedInterstitialImpressionRec;
                rewardedInterstitial.OnAdClicked -= RewardedInterstitialAdClicked;
                rewardedInterstitial.OnAdFullScreenContentOpened -= RewardedInterstitialFullScreenOpened;
                rewardedInterstitial.OnAdFullScreenContentClosed -= RewardedInterstitialFullScreenClosed;
                rewardedInterstitial.OnAdFullScreenContentFailed -= RewardedInterstitialFullScreenFailed;
                rewardedInterstitial.Destroy();
                rewardedInterstitial = null;
            }

            // create our request used to load the ad.
            var adRequest = new AdRequest();

            // send the request to load the ad.
            RewardedInterstitialAd.Load(rewardedInterstitialId, adRequest, LoadRewardedInterstitialCallback);
        }

        private void LoadRewardedInterstitialCallback(RewardedInterstitialAd ad, LoadAdError loadAdError)
        {
            // if error is not null, the load request failed.
            if (loadAdError != null || ad == null)
            {
                RewardedInterstitialFailed(loadAdError);
                return;
            }
            else
            {
                RewardedInterstitialLoaded(ad);
            }
        }

        private void RewardedInterstitialLoaded(RewardedInterstitialAd ad)
        {
            rewardedInterstitial = ad;
            BotanGameServicesLogger.AddLog("Rewarded Interstitial Loaded.");

            rewardedInterstitial.OnAdPaid += RewardedInterstitialPaid;
            rewardedInterstitial.OnAdImpressionRecorded += RewardedInterstitialImpressionRec;
            rewardedInterstitial.OnAdClicked += RewardedInterstitialAdClicked;
            rewardedInterstitial.OnAdFullScreenContentOpened += RewardedInterstitialFullScreenOpened;
            rewardedInterstitial.OnAdFullScreenContentClosed += RewardedInterstitialFullScreenClosed;
            rewardedInterstitial.OnAdFullScreenContentFailed += RewardedInterstitialFullScreenFailed;

            currentRetryRewardedInterstitial = 0;
            events.TriggerRewardedInterstitialLoadSucces();
        }

        private void RewardedInterstitialFailed(LoadAdError e)
        {
            BotanGameServicesLogger.AddLog($"Rewarded interstitial ad failed to load an ad with error : {e}");


            //try again to load a rewarded video
            if (currentRetryRewardedInterstitial < maxRetryCount)
            {
                currentRetryRewardedInterstitial++;

                BotanGameServicesLogger.AddLog("Retry " + currentRetryRewardedInterstitial);

                rewardedInterstitialFailedToLoad = true;
            }
            events.TriggerRewardedInterstitialLoadFailed(e.ToString());
        }

        private void RewardedInterstitialFullScreenFailed(AdError error)
        {

            BotanGameServicesLogger.AddLog($"Rewarded Interstitial ad failed to open full screen content. Error: {error}");

            //Reload the rewarded ad so that we can show another one as soon as possible
            RewardedInterstitialClosed();
        }

        private void RewardedInterstitialFullScreenClosed()
        {
            BotanGameServicesLogger.AddLog("Rewarded Interstitial ad full screen content closed.");
#if !UNITY_EDITOR
            RewardedInterstitialClosed();
#endif
        }

        private void RewardedInterstitialFullScreenOpened()
        {
            BotanGameServicesLogger.AddLog("Rewarded Interstitial ad full screen content opened.");
        }

        private void RewardedInterstitialAdClicked()
        {
            BotanGameServicesLogger.AddLog("Rewarded Interstitial ad was clicked.");
            events.TriggerRewardedInterstitialClicked();
        }

        private void RewardedInterstitialImpressionRec()
        {
            BotanGameServicesLogger.AddLog("Rewarded Interstitial ad recorded an impression.");
        }

        private void RewardedInterstitialPaid(AdValue adValue)
        {
            BotanGameServicesLogger.AddLog($"Rewarded Interstitial ad paid {adValue.Value} {adValue.CurrencyCode}");
        }


        #endregion


        #region MRec
        public void ShowMRec(BannerPosition position, Vector2Int customPosition)
        {
            ShowBanner(mrecId, position, BannerType.MediumRectangle, new Vector2Int(), customPosition, false);
        }

        public void HideMRec()
        {
            HideBanner();
        }
        #endregion


        #region Resume
        public void LoadAdsOnResume()
        {
            if (initialized == true)
            {
                if (IsInterstitialAvailable() == false)
                {
                    if (currentRetryInterstitial == maxRetryCount)
                    {
                        LoadInterstitial();
                    }
                }

                if (IsRewardedVideoAvailable() == false)
                {
                    if (currentRetryRewardedVideo == maxRetryCount)
                    {
                        LoadRewardedVideo();
                    }
                }

                if (IsRewardedInterstitialAvailable() == false)
                {
                    if (currentRetryRewardedInterstitial == maxRetryCount)
                    {
                        LoadRewardedInterstitial();
                    }
                }

                if (IsAppOpenAvailable() == false)
                {
                    if (currentRetryAppOpen == maxRetryCount)
                    {
                        LoadAppOpen();
                    }
                }
            }
        }
        #endregion


        #region AutoConsent
        #region InterfaceImplementation
        public bool HasBuiltInConsentWindow()
        {
            //ConsentInformation.Reset();
            return true;
        }

        public void InitializaConsentWindow(DebugGeography debugGeography, string testDevice)
        {
            testDeviceID = testDevice;
            this.debugGeography = debugGeography;
        }

        public void ShowBuiltInConsentWindow(UnityAction consentPopupClosed)
        {
            onConsentPopupClosed = consentPopupClosed;
            if (GDPRConsentWasSet() == false)
            {
                SetInitialConsent(directedForChildren);
            }
            else
            {
                ShowForm();
            }
        }
        #endregion

        private void SetInitialConsent(bool directedForChildren)
        {
            var debugSettings = new ConsentDebugSettings
            {
                // Geography appears as in EEA for debug devices.
                DebugGeography = debugGeography,
            };

            if (!string.IsNullOrEmpty(testDeviceID))
            {
                debugSettings.TestDeviceHashedIds.Add(testDeviceID);
            }

            ConsentRequestParameters request = new ConsentRequestParameters
            {
                TagForUnderAgeOfConsent = directedForChildren,
                ConsentDebugSettings = debugSettings,
            };
            // Check the current consent information status.
            ConsentInformation.Update(request, OnConsentInfoUpdated);
        }

        private void OnConsentInfoUpdated(FormError error)
        {
            if (error != null)
            {
                OnConsentPopupClosed(error);
                return;
            }
            // If the error is null, the consent information state was updated.
            // You are now ready to check if a form is available.

            LoadForm();
        }


        private void LoadForm()
        {
            if (ConsentInformation.IsConsentFormAvailable())
            {
                // Loads a consent form.
                ConsentForm.Load(OnLoadConsentForm);
            }
            else
            {
                BotanGameServicesLogger.AddLog("Consent form not available");
                OnConsentPopupClosed(null);
            }
        }

        private void OnLoadConsentForm(ConsentForm consentForm, FormError error)
        {
            if (error != null)
            {
                // Handle the error.
                OnConsentPopupClosed(error);
                return;
            }
            BotanGameServicesLogger.AddLog($"Consent Status: {ConsentInformation.ConsentStatus}");
            // The consent form was loaded.
            // Save the consent form for future requests.
            _consentForm = consentForm;

            if (ConsentInformation.ConsentStatus == ConsentStatus.Required)
            {
                ShowForm();
            }
            else
            {
                BotanGameServicesLogger.AddLog("Consent form not required");
                OnConsentPopupClosed(null);
            }
        }


        private void ShowForm()
        {
            if (_consentForm != null)
            {
                _consentForm.Show(OnShowForm);
            }
            else
            {
                LoadForm();
            }
        }


        private void OnShowForm(FormError error)
        {
            if (error != null)
            {
                // Handle the error.
                OnConsentPopupClosed(error);
                return;
            }

            // Handle dismissal by reloading form.
            BotanGameServicesLogger.AddLog("Form Closed");
            OnConsentPopupClosed(null);
            LoadForm();
        }

        void OnConsentPopupClosed(FormError error)
        {
            if (error != null)
            {
                BotanGameServicesLogger.AddLog($"{error.ErrorCode} {error.Message}");
            }
            onConsentPopupClosed?.Invoke();
            onConsentPopupClosed = null;
        }
        #endregion


        #region ManualConsent
        public bool GDPRConsentWasSet()
        {
            if (ConsentInformation.ConsentStatus == ConsentStatus.Obtained || ConsentInformation.ConsentStatus == ConsentStatus.NotRequired)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public void SetGDPRConsent(bool accept)
        {
            BotanGameServicesLogger.AddLog($"Admob does not support SetGDPRConsent. Use build in consent popup for settings");
        }

        public UserConsent GetGDPRConsent()
        {
            switch (ConsentInformation.ConsentStatus)
            {
                case ConsentStatus.Obtained:
                case ConsentStatus.NotRequired:
                    return UserConsent.Accept;

                default:
                    return UserConsent.Unset;
            }
        }

        public bool CCPAConsentWasSet()
        {
            return GDPRConsentWasSet();
        }

        public void SetCCPAConsent(bool accept)
        {
            BotanGameServicesLogger.AddLog($"Admob does not support SetCCPAConsent. Use build in consent popup for settings");
        }

        public UserConsent GetCCPAConsent()
        {
            return GetGDPRConsent();
        }
        #endregion


        #region Debug
        public void OpenDebugWindow()
        {
            MobileAds.OpenAdInspector((AdInspectorError error) =>
            {
                // If the operation failed, an error is returned.
                if (error != null)
                {
                    BotanGameServicesLogger.AddLog($"Ad Inspector failed to open with error: {error}");
                    return;
                }

                Debug.Log("Ad Inspector opened successfully.");
            });
        }
        #endregion


        AdRequest CreateRequest()
        {
            AdRequest request = new AdRequest();
            request.Extras.Add("is_designed_for_families", designedForFamilies);
            return request;
        }


        void LogErrorMessage(LoadAdError loadAdError)
        {
            // Gets the error code. See
            // https://developers.google.com/android/reference/com/google/android/gms/ads/AdRequest
            // and https://developers.google.com/admob/ios/api/reference/Enums/GADErrorCode
            // for a list of possible codes.

            // Gets an error message.
            // For example "Account not approved yet". See
            // https://support.google.com/admob/answer/9905175 for explanations of
            // common errors.
            BotanGameServicesLogger.AddLog(loadAdError);
            var responseInfo = loadAdError.GetResponseInfo();
            if (responseInfo != null)
            {
                BotanGameServicesLogger.AddLog(responseInfo.ToString());

                foreach (var item in responseInfo.GetAdapterResponses())
                {
                    BotanGameServicesLogger.AddLog($"{item.AdapterClassName} {item.AdError} {item.AdError.GetMessage()} {item.AdError.GetCode()}");
                }
            }
        }


        /// <summary>
        /// Used to delay the admob events for multi-threading applications
        /// </summary>
        private void Update()
        {
            if (interstitialFailedToLoad)
            {
                interstitialFailedToLoad = false;
                Invoke("LoadInterstitial", reloadTime);
            }

            if (rewardedVideoFailedToLoad)
            {
                rewardedVideoFailedToLoad = false;
                Invoke("LoadRewardedVideo", reloadTime);
            }

            if (rewardedInterstitialFailedToLoad)
            {
                rewardedInterstitialFailedToLoad = false;
                Invoke("LoadRewardedInterstitial", reloadTime);
            }

            if (appOpenFailedToLoad)
            {
                appOpenFailedToLoad = false;
                Invoke("LoadAppOpen", reloadTime);
            }
        }

        //private void Awake()
        //{
        //    // Use the AppStateEventNotifier to listen to application open/close events.
        //    // This is used to launch the loaded ad when we open the app.
        //    AppStateEventNotifier.AppStateChanged += OnAppStateChanged;
        //}

        //private void OnDestroy()
        //{
        //    // Always unlisten to events when complete.
        //    AppStateEventNotifier.AppStateChanged -= OnAppStateChanged;
        //}

        //private void OnAppStateChanged(AppState state)
        //{
        //    Debug.Log("App State changed to : " + state);

        //    // if the app is Foregrounded and the ad is available, show it.
        //    if (state == AppState.Foreground)
        //    {

        //        StartCoroutine(OpenApp());
        //    }
        //}

        //IEnumerator OpenApp()
        //{
        //    yield return new WaitForSeconds(0.2f);
        //    ShowAppOpen();
        //}
    }
}
#endif