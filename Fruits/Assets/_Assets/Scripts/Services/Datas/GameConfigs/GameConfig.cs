using System;
using _Assets.Scripts.Services.Audio;

namespace _Assets.Scripts.Services.Datas.GameConfigs
{
    public struct GameConfig : IComparable<GameConfig>
    {
        public string ModName;
        public string ModIconPath;
        public string ContainerImagePath;
        public string[] FruitSkinsImagesPaths;
        public string[] FruitIconsPaths;
        public AudioData[] FruitAudios;
        public float[] FruitDropChances;
        public float TimeBeforeTimerTrigger;
        public float TimerStartTime;
        public string InGameBackgroundPath;
        public string LoadingScreenBackgroundPath;
        public string LoadingScreenIconPath;
        public string PlayerSkinPath;
        public AudioData[] MergeSoundsAudios;
        public string MainMenuBackgroundPath;

        public int CompareTo(GameConfig other)
        {
            var modNameComparison = string.Compare(ModName, other.ModName, StringComparison.Ordinal);
            if (modNameComparison != 0) return modNameComparison;
            var modIconPathComparison = string.Compare(ModIconPath, other.ModIconPath, StringComparison.Ordinal);
            if (modIconPathComparison != 0) return modIconPathComparison;
            var containerImagePathComparison = string.Compare(ContainerImagePath, other.ContainerImagePath, StringComparison.Ordinal);
            if (containerImagePathComparison != 0) return containerImagePathComparison;
            var timeBeforeTimerTriggerComparison = TimeBeforeTimerTrigger.CompareTo(other.TimeBeforeTimerTrigger);
            if (timeBeforeTimerTriggerComparison != 0) return timeBeforeTimerTriggerComparison;
            var timerStartTimeComparison = TimerStartTime.CompareTo(other.TimerStartTime);
            if (timerStartTimeComparison != 0) return timerStartTimeComparison;
            var inGameBackgroundPathComparison = string.Compare(InGameBackgroundPath, other.InGameBackgroundPath, StringComparison.Ordinal);
            if (inGameBackgroundPathComparison != 0) return inGameBackgroundPathComparison;
            var loadingScreenBackgroundPathComparison = string.Compare(LoadingScreenBackgroundPath, other.LoadingScreenBackgroundPath, StringComparison.Ordinal);
            if (loadingScreenBackgroundPathComparison != 0) return loadingScreenBackgroundPathComparison;
            var loadingScreenIconPathComparison = string.Compare(LoadingScreenIconPath, other.LoadingScreenIconPath, StringComparison.Ordinal);
            if (loadingScreenIconPathComparison != 0) return loadingScreenIconPathComparison;
            var playerSkinPathComparison = string.Compare(PlayerSkinPath, other.PlayerSkinPath, StringComparison.Ordinal);
            if (playerSkinPathComparison != 0) return playerSkinPathComparison;
            return string.Compare(MainMenuBackgroundPath, other.MainMenuBackgroundPath, StringComparison.Ordinal);
        }
    }
}