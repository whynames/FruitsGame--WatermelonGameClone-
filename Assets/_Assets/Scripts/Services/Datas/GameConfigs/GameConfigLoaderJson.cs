using System;
using System.Collections.Generic;
using System.IO;
using _Assets.Scripts.Misc;
using Cysharp.Threading.Tasks;
using Newtonsoft.Json;
using UnityEngine;
using UnityEngine.Networking;

namespace _Assets.Scripts.Services.Datas.GameConfigs
{
    public class GameConfigLoaderJson : IConfigLoader
    {
        private readonly GameConfigValidator _gameConfigValidator;
        private readonly SpritesCacheService _spritesCacheService;
        private GameConfig _currentConfig;
        private readonly List<GameConfig> _allConfigs = new();

        public List<GameConfig> AllConfigs => _allConfigs;
        public GameConfig CurrentConfig => _currentConfig;
        public bool IsDefault => _currentConfig.Equals(_allConfigs[0]);
        public event Action<GameConfig> ConfigChanged;

        public GameConfigLoaderJson(GameConfigValidator gameConfigValidator, SpritesCacheService spritesCacheService)
        {
            _gameConfigValidator = gameConfigValidator;
            _spritesCacheService = spritesCacheService;
        }

        public async UniTask LoadDefaultConfig()
        {
#if UNITY_WEBGL
            // WebGL-specific loading from StreamingAssets using UnityWebRequest
            var targetFile = Path.Combine(Application.streamingAssetsPath, PathsHelper.ConfigJson);
            UnityWebRequest www = UnityWebRequest.Get(targetFile);
            await www.SendWebRequest();

            if (www.result != UnityWebRequest.Result.Success)
            {
                Debug.LogError(www.error);
                return;
            }

            var json = www.downloadHandler.text;
            var config = JsonConvert.DeserializeObject<GameConfig>(json);
            UpdateRelativePaths(config);
            _allConfigs.Add(config);
            _currentConfig = config;
#else
            // Non-WebGL loading from file system
            var targetFile = Path.Combine(PathsHelper.StreamingAssetsPath, PathsHelper.ConfigJson);
            if (!File.Exists(targetFile))
            {
                Debug.LogWarning("Config file not found at path: " + targetFile);
                return;
            }

            using (var reader = new StreamReader(targetFile))
            {
                var json = await reader.ReadToEndAsync();
                var config = JsonConvert.DeserializeObject<GameConfig>(json);
                UpdateRelativePaths(config);
                _allConfigs.Add(config);
                _currentConfig = config;
            }
#endif

            if (_currentConfig.TimeBeforeTimerTrigger < 0)
                Debug.LogError("TimeBeforeTimerTrigger is less than 0. Setting from default config.");

            if (_currentConfig.TimerStartTime < 0)
                Debug.LogError("TimerStartTime is less than 0. Setting from default config.");

            ConfigChanged?.Invoke(_currentConfig);
            await UniTask.Delay(100);
        }

        public void LoadAllConfigs()
        {
#if !UNITY_WEBGL
            // Only for non-WebGL due to filesystem usage
            var modsFoldersDirectoryInfo = new DirectoryInfo(PathsHelper.ModsPath);

            if (!modsFoldersDirectoryInfo.Exists)
            {
                modsFoldersDirectoryInfo.Create();
                return;
            }

            foreach (var directoryInfo in modsFoldersDirectoryInfo.GetDirectories())
            {
                foreach (var fileInfo in directoryInfo.GetFiles(PathsHelper.ConfigJson))
                {
                    using (var reader = new StreamReader(fileInfo.FullName))
                    {
                        var json = reader.ReadToEnd();
                        var config = JsonConvert.DeserializeObject<GameConfig>(json);

                        var defaultConfig = _allConfigs[0];
                        _gameConfigValidator.Validate(ref defaultConfig, ref config);

                        _allConfigs.Add(config);
                    }
                }
            }
#endif
        }

        public void SetCurrentConfig(string modName)
        {
            _spritesCacheService.Reset();

            foreach (var config in _allConfigs)
            {
                if (config.ModName == modName)
                {
                    _currentConfig = config;
                    break;
                }
            }

            ConfigChanged?.Invoke(_currentConfig);
        }

        private void UpdateRelativePaths(GameConfig config)
        {
            config.ModIconPath = Path.Combine(PathsHelper.StreamingAssetsPath, config.ModIconPath);
            config.ContainerImagePath = Path.Combine(PathsHelper.StreamingAssetsPath, config.ContainerImagePath);

            for (int i = 0; i < config.FruitSkinsImagesPaths.Length; i++)
            {
                config.FruitSkinsImagesPaths[i] = Path.Combine(PathsHelper.StreamingAssetsPath, config.FruitSkinsImagesPaths[i]);
            }

            for (int i = 0; i < config.FruitIconsPaths.Length; i++)
            {
                config.FruitIconsPaths[i] = Path.Combine(PathsHelper.StreamingAssetsPath, config.FruitIconsPaths[i]);
            }

            for (int i = 0; i < config.FruitAudios.Length; i++)
            {
                config.FruitAudios[i].Path = Path.Combine(PathsHelper.StreamingAssetsPath, config.FruitAudios[i].Path);
            }

            config.InGameBackgroundPath = Path.Combine(PathsHelper.StreamingAssetsPath, config.InGameBackgroundPath);
            config.LoadingScreenBackgroundPath = Path.Combine(PathsHelper.StreamingAssetsPath, config.LoadingScreenBackgroundPath);
            config.LoadingScreenIconPath = Path.Combine(PathsHelper.StreamingAssetsPath, config.LoadingScreenIconPath);
            config.PlayerSkinPath = Path.Combine(PathsHelper.StreamingAssetsPath, config.PlayerSkinPath);

            for (int i = 0; i < config.MergeSoundsAudios.Length; i++)
            {
                config.MergeSoundsAudios[i].Path = Path.Combine(PathsHelper.StreamingAssetsPath, config.MergeSoundsAudios[i].Path);
            }

            config.MainMenuBackgroundPath = Path.Combine(PathsHelper.StreamingAssetsPath, config.MainMenuBackgroundPath);
        }
    }
}
