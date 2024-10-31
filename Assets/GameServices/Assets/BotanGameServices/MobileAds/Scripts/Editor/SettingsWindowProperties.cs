using BotanGameServices.Main;

namespace BotanGameServices.AdsPackage.Editor
{
    public class SettingsWindowProperties : ISettingsWindowProperties
    {
        public const string menuItem = "Tools/BotanGameServices/Mobile Ads";
        public const string testScene = "Example/Scenes/MobileAdsTest.unity";
        public const string exampleScene = "Example/Scenes/MobileAdsExample.unity";
        public const string documentation = "https://BotanGameServices.gitbook.io/mobile-ads/";

        public const string BOTANGAMESERVICES_ADCOLONY = "BotanGameServices_ADCOLONY";
        public const string BOTANGAMESERVICES_ADMOB = "BotanGameServices_ADMOB";
        public const string BOTANGAMESERVICES_CHARTBOOST = "BotanGameServices_CHARTBOOST";
        public const string BOTANGAMESERVICES_UNITYADS = "BotanGameServices_UNITYADS";
        public const string BOTANGAMESERVICES_VUNGLE = "BotanGameServices_VUNGLE";
        public const string BOTANGAMESERVICES_APPLOVIN = "BotanGameServices_APPLOVIN";
        public const string BOTANGAMESERVICES_LEVELPLAY = "BotanGameServices_LEVELPLAY";
        public const string BOTANGAMESERVICES_ATT = "BotanGameServices_ATT";
        public const string BOTANGAMESERVICES_PATCH_ADMOB = "BotanGameServices_PATCH_ADMOB";

        public string versionFilePath => "/Scripts/Version.txt";
        public string windowName => "Mobile Ads - v.";
        public int minWidth => 520;
        public int minHeight => 520;
        public string folderName => "MobileAds";
        public string parentFolder => "BotanGameServices";
    }
}
