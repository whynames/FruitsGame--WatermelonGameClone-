using BotanGameServices.Main;

namespace BotanGameServices.GameServices.Editor
{
    public class SettingsWindowProperties : ISettingsWindowProperties
    {
        public const string menuItem = "Tools/BotanGameServices/Game Services";
        public const string BotanGameServices_GAMESERVICES_ANDROID = "BotanGameServices_GAMESERVICES_ANDROID";
        public const string BotanGameServices_GAMESERVICES_IOS = "BotanGameServices_GAMESERVICES_IOS";
        internal static object gameServicesExample = "Example/Scenes/GameServicesExample.unity";
        internal static object gameServicesTest = "Example/Scenes/GameServicesTest.unity";
        internal static string documentation = "https://BotanGameServices.gitbook.io/easy-achievements/";

        public string versionFilePath => "/Scripts/Version.txt";

        public string windowName => "Game Services - v.";

        public int minWidth => 520;

        public int minHeight => 520;

        public string folderName => "GameServices";

        public string parentFolder => "BotanGameServices";
    }
}