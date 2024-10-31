using BotanGameServices.Main;

namespace BotanGameServices.DailyRewards.Editor
{
    public class SettingsWindowProperties : ISettingsWindowProperties
    {
        internal const string menuItem = "Tools/BotanGameServices/Daily Rewards";

        internal const string BotanGameServices_DAILY_REWARDS = "BotanGameServices_DAILY_REWARDS";
        internal const string documentation = "https://BotanGameServices.gitbook.io/daily-rewards/";
        internal const string timerButtonExample = "Example/Scenes/TimerButtonExample.unity";
        internal const string calendarExample = "Example/Scenes/CalendarExample.unity";

        public string versionFilePath => "/Scripts/Version.txt";

        public string windowName => "Daily Rewards - v.";

        public int minWidth => 520;

        public int minHeight => 520;

        public string folderName => "DailyRewards";

        public string parentFolder => "BotanGameServices";
    }
}