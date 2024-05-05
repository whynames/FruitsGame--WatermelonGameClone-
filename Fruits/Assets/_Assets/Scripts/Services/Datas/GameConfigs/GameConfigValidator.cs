using System;
using System.IO;
using System.Linq;
using _Assets.Scripts.Misc;
using Newtonsoft.Json;
using UnityEngine;

namespace _Assets.Scripts.Services.Datas.GameConfigs
{
    public class GameConfigValidator
    {
        public void Validate(ref GameConfig defaultConfig, ref GameConfig config)
        {
            ValidateModIconPath(ref defaultConfig, ref config);
            ValidateContainerImagePath(ref defaultConfig, ref config);
            ValidateFruitSkinsImages(ref defaultConfig, ref config);
            ValidateFruitIcons(ref defaultConfig, ref config);
            ValidateFruitAudio(ref defaultConfig, ref config);
            ValidateFruitDropChances(ref defaultConfig, ref config);
            ValidateTimeBeforeTimerTrigger(ref defaultConfig, ref config);
            ValidateTimerStartTime(ref defaultConfig, ref config);
            ValidateInGameBackground(ref defaultConfig, ref config);
            ValidateLoadingScreenBackground(ref defaultConfig, ref config);
            ValidateLoadingScreenIcon(ref defaultConfig, ref config);
            ValidatePlayerSkin(ref defaultConfig, ref config);
            ValidateMergeSoundsAudios(ref defaultConfig, ref config);
            ValidateMainMenuBackground(ref defaultConfig, ref config);
        }

        private void ValidateModIconPath(ref GameConfig defaultConfig, ref GameConfig config)
        {
            config.ModIconPath = GetFilePath(config.ModIconPath, defaultConfig.ModIconPath, config.ModName);
        }

        private void ValidateContainerImagePath(ref GameConfig defaultConfig, ref GameConfig config)
        {
            config.ContainerImagePath =
                GetFilePath(config.ContainerImagePath, defaultConfig.ContainerImagePath, config.ModName);
        }

        private void ValidateFruitSkinsImages(ref GameConfig defaultConfig, ref GameConfig config)
        {
            for (int i = 0; i < config.FruitSkinsImagesPaths.Length; i++)
            {
                config.FruitSkinsImagesPaths[i] = GetFilePath(config.FruitSkinsImagesPaths[i],
                    defaultConfig.FruitSkinsImagesPaths[i], config.ModName);
            }
        }

        private void ValidateFruitIcons(ref GameConfig defaultConfig, ref GameConfig config)
        {
            for (int i = 0; i < config.FruitIconsPaths.Length; i++)
            {
                config.FruitIconsPaths[i] = GetFilePath(config.FruitIconsPaths[i], defaultConfig.FruitIconsPaths[i],
                    config.ModName);
            }
        }

        private void ValidateFruitAudio(ref GameConfig defaultConfig, ref GameConfig config)
        {
            for (int i = 0; i < config.FruitAudios.Length; i++)
            {
                config.FruitAudios[i].Path = GetFilePath(config.FruitAudios[i].Path, defaultConfig.FruitAudios[i].Path, config.ModName);
            }
        }

        private void ValidateFruitDropChances(ref GameConfig defaultConfig, ref GameConfig config)
        {
            if (config.FruitDropChances == null)
            {
                Debug.LogError($"Mod: {config.ModName} Missing drop chance. Setting from default config");

                config.FruitDropChances = new float[defaultConfig.FruitDropChances.Length];

                for (int i = 0; i < defaultConfig.FruitDropChances.Length; i++)
                {
                    config.FruitDropChances[i] = defaultConfig.FruitDropChances[i];
                }

                return;
            }

            if (config.FruitDropChances.Length < defaultConfig.FruitDropChances.Length)
            {
                var originalLength = config.FruitDropChances.Length;
                Array.Resize(ref config.FruitDropChances, defaultConfig.FruitDropChances.Length);

                for (int i = originalLength; i < config.FruitDropChances.Length; i++)
                {
                    config.FruitDropChances[i] = defaultConfig.FruitDropChances[i];
                    Debug.LogError(
                        $"Mod: {config.ModName} Missing drop chance at index {i}. Setting from default config. Value: {defaultConfig.FruitDropChances[i]}");
                }
            }
            else if (config.FruitDropChances.Length != defaultConfig.FruitDropChances.Length)
            {
                Debug.LogError(
                    $"Mod: {config.ModName} have {config.FruitDropChances.Length} drop chances. But should have {defaultConfig.FruitDropChances.Length}. Trimming");
                config.FruitDropChances = config.FruitDropChances.Take(defaultConfig.FruitDropChances.Length).ToArray();
            }
        }

        private void ValidateTimeBeforeTimerTrigger(ref GameConfig defaultConfig, ref GameConfig config)
        {
            if (config.TimeBeforeTimerTrigger < 0)
            {
                Debug.LogError(
                    $"Mod: {config.ModName} TimeBeforeTimerTrigger is less than 0. Setting from default config. Value: {defaultConfig.TimeBeforeTimerTrigger}");
                config.TimeBeforeTimerTrigger = defaultConfig.TimeBeforeTimerTrigger;
            }
        }

        private void ValidateTimerStartTime(ref GameConfig defaultConfig, ref GameConfig config)
        {
            if (config.TimerStartTime < 0)
            {
                Debug.LogError(
                    $"Mod: {config.ModName} TimerStartTime is less than 0. Setting from default config. Value: {defaultConfig.TimeBeforeTimerTrigger}");
                config.TimerStartTime = defaultConfig.TimerStartTime;
            }
        }

        private void ValidateInGameBackground(ref GameConfig defaultConfig, ref GameConfig config)
        {
            config.InGameBackgroundPath = GetFilePath(config.InGameBackgroundPath, defaultConfig.InGameBackgroundPath,
                config.ModName);
        }

        private void ValidateLoadingScreenBackground(ref GameConfig defaultConfig, ref GameConfig config)
        {
            config.LoadingScreenBackgroundPath = GetFilePath(config.LoadingScreenBackgroundPath, defaultConfig.LoadingScreenBackgroundPath,
                config.ModName);
        }

        private void ValidateLoadingScreenIcon(ref GameConfig defaultConfig, ref GameConfig config)
        {
            config.LoadingScreenIconPath = GetFilePath(config.LoadingScreenIconPath, defaultConfig.LoadingScreenIconPath,
                config.ModName);
        }

        private void ValidatePlayerSkin(ref GameConfig defaultConfig, ref GameConfig config)
        {
            config.PlayerSkinPath = GetFilePath(config.PlayerSkinPath, defaultConfig.PlayerSkinPath,
                config.ModName);
        }

        private void ValidateMergeSoundsAudios(ref GameConfig defaultConfig, ref GameConfig config)
        {
            for (int i = 0; i < config.MergeSoundsAudios.Length; i++)
            {
                config.MergeSoundsAudios[i].Path = GetFilePath(config.MergeSoundsAudios[i].Path, defaultConfig.MergeSoundsAudios[i].Path,
                    config.ModName);
            }
        }

        private void ValidateMainMenuBackground(ref GameConfig defaultConfig, ref GameConfig config)
        {
            config.MainMenuBackgroundPath = GetFilePath(config.MainMenuBackgroundPath, defaultConfig.MainMenuBackgroundPath,
                config.ModName);
        }

        private string GetFilePath(string filePath, string defaultPath, string modName)
        {
            if (string.IsNullOrEmpty(filePath) || string.IsNullOrEmpty(modName) ||
                string.IsNullOrWhiteSpace(filePath) || string.IsNullOrWhiteSpace(modName))
            {
                Debug.LogError($"Mod: {modName} File {filePath} not found. Setting from default config");
                return defaultPath;
            }

            var fullPath = Path.Combine(PathsHelper.ModsPath, modName, filePath);
            if (!File.Exists(fullPath))
            {
                Debug.LogError($"Mod: {modName} File {fullPath} not found. Setting from default config");
                return defaultPath;
            }

            return fullPath;
        }

        private void Save(ref GameConfig gameConfig)
        {
            Debug.LogError($"SAVING MOD CONFIG {gameConfig.FruitDropChances.Length}");

            var json = JsonConvert.SerializeObject(gameConfig);
            var filePath = Path.Combine(gameConfig.ModName, "config.json");

            using (StreamWriter streamWriter = new StreamWriter(filePath))
            {
                streamWriter.Write(json);
            }
        }
    }
}