using BotanGameServices.Main;

namespace BotanGameServices.IAPSystem.Editor
{
    public class SettingsWindowProperties : ISettingsWindowProperties
    {
        public const string menuItem = "Tools/BotanGameServices/IAPSystem";

        public const string BOTANGAMESERVICES_IAP_IOS = "BOTANGAMESERVICES_IAP_IOS";
        public const string BOTANGAMESERVICES_IAP_GOOGLEPLAY = "BOTANGAMESERVICES_IAP_GOOGLEPLAY";
        public const string BOTANGAMESERVICES_IAP_AMAZON = "BOTANGAMESERVICES_IAP_AMAZON";
        public const string BOTANGAMESERVICES_IAP_MACOS = "BOTANGAMESERVICES_IAP_MACOS";
        public const string BOTANGAMESERVICES_IAP_WINDOWS = "BOTANGAMESERVICES_IAP_WINDOWS";
        public const string BOTANGAMESERVICES_IAP_VALIDATION = "BOTANGAMESERVICES_IAP_VALIDATION";
        internal const string exampleScene = "Example/Scenes/EasyIAPExample.unity";
        internal const string testScene = "Example/Scenes/EasyIAPTest.unity";


        public string versionFilePath => "/Scripts/Version.txt";

        public string windowName => "Easy IAP - v.";

        public int minWidth => 520;

        public int minHeight => 520;

        public string folderName => "IAPSystem";

        public string parentFolder => "BotanGameServices";
    }
}
