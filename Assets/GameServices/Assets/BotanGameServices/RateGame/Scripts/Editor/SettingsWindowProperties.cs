using BotanGameServices.Main;

namespace BotanGameServices.RateGame.Editor
{
    public class SettingsWindowProperties : ISettingsWindowProperties
    {
        public const string menuItem = "Tools/BotanGameServices/Rate Game";

        public const string BotanGameServices_NATIVE_GOOGLEPLAY = "BotanGameServices_NATIVE_GOOGLEPLAY";
        public const string BotanGameServices_NATIVE_APPSTORE = "BotanGameServices_NATIVE_APPSTORE";
        internal const string documentation = "https://BotanGameServices.gitbook.io/rate-game/";
        internal const string exampleScene = "Example/Scenes/RateGameExample.unity";

        public string versionFilePath => "/Scripts/Version.txt";

        public string windowName => "Rate Game - v.";

        public int minWidth => 520;

        public int minHeight => 520;

        public string folderName => "RateGame";

        public string parentFolder => "BotanGameServices";
    }
}