using BotanGameServices.Main;

namespace BotanGameServices.Notifications.Editor
{
    public class SettingsWindowProperties : ISettingsWindowProperties
    {
        public const string menuItem = "Tools/BotanGameServices/Notifications";

        public const string BotanGameServices_NOTIFICATIONS_ANDROID = "BotanGameServices_NOTIFICATIONS_ANDROID";
        public const string BotanGameServices_NOTIFICATIONS_IOS = "BotanGameServices_NOTIFICATIONS_IOS";
        internal const string notificationExample = "Example/Scenes/NotificationsExample.unity";
        internal static string documentation = "https://BotanGameServices.gitbook.io/mobile-notifications/";

        public string versionFilePath => "/Scripts/Version.txt";

        public string windowName => "Notifications - v.";

        public int minWidth => 520;

        public int minHeight => 520;

        public string folderName => "Notifications";

        public string parentFolder => "BotanGameServices";
    }
}