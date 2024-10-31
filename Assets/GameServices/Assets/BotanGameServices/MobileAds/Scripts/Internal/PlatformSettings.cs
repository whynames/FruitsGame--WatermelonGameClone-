namespace BotanGameServices.AdsPackage.Local
{
#if BOTANGAMESERVICES_ADMOB
    using GoogleMobileAds.Ump.Api;
#endif
    [System.Serializable]
    public class PlatformSettings
    {
        public SupportedPlatforms platform;
        public bool enabled;
        public AdUnitID appId;
        public AdUnitID idBanner;
        public AdUnitID idMRec;
        public AdUnitID idInterstitial;
        public AdUnitID idRewarded;
        public AdUnitID idRewardedInterstitial;
        public AdUnitID idOpenApp;
        public bool testMode;
        public string testDevice;
#if BOTANGAMESERVICES_ADMOB
        public DebugGeography debugGeography;
#endif
        public PlatformSettings(SupportedPlatforms platform, AdUnitID appId, AdUnitID idBanner, AdUnitID idInterstitial, AdUnitID idRewarded, AdUnitID idMRec, AdUnitID idRewardedInterstitial, AdUnitID idOpenApp)
        {
            this.platform = platform;
            this.appId = appId;
            this.idBanner = idBanner;
            this.idInterstitial = idInterstitial;
            this.idRewarded = idRewarded;
            this.idMRec = idMRec;
            this.idRewardedInterstitial = idRewardedInterstitial;
            this.idOpenApp = idOpenApp;
        }
    }
}