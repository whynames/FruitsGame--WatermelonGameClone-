using System.IO;
using _Assets.Scripts.Misc;
using Cysharp.Threading.Tasks;
using Newtonsoft.Json;
using UnityEngine;

namespace _Assets.Scripts.Services.Audio
{
    public class AudioSettingsDataLoaderJson : IAudioSettingsLoader
    {
        private const string AudioSettingsKey = "AudioSettingsData";
        private AudioSettingsData _audioSettingsData;
        public AudioSettingsData AudioData => _audioSettingsData;

        public void ChangeSoundVolume(float volume) => _audioSettingsData.VFXVolume = volume;

        public void ChangeMusicVolume(float volume) => _audioSettingsData.MusicVolume = volume;

        public async UniTask Load()
        {
#if UNITY_WEBGL
            // WebGL-specific loading from PlayerPrefs
            if (PlayerPrefs.HasKey(AudioSettingsKey))
            {
                var json = PlayerPrefs.GetString(AudioSettingsKey);
                _audioSettingsData = JsonConvert.DeserializeObject<AudioSettingsData>(json);
            }
            else
            {
                // Default settings if no data is found
                _audioSettingsData = new AudioSettingsData(0.5f, 0.5f);
                Debug.LogWarning("Audio settings not found. Using default settings.");
            }
#else
            // Non-WebGL file-based loading
            var dataFolderInfo = new DirectoryInfo(PathsHelper.DataPath);

            if (!File.Exists(Path.Combine(PathsHelper.DataPath, PathsHelper.AudioSettingsDataJson)))
            {
                _audioSettingsData = new AudioSettingsData(0.5f, 0.5f);
                Debug.LogWarning("Audio settings not found");
                return;
            }

            foreach (var fileInfo in dataFolderInfo.GetFiles(PathsHelper.AudioSettingsDataJson))
            {
                using (var reader = new StreamReader(fileInfo.FullName))
                {
                    var json = await reader.ReadToEndAsync();
                    _audioSettingsData = JsonConvert.DeserializeObject<AudioSettingsData>(json);
                }
            }
#endif
        }

        public void Save()
        {
#if UNITY_WEBGL
            // WebGL-specific saving to PlayerPrefs
            var json = JsonConvert.SerializeObject(_audioSettingsData);
            PlayerPrefs.SetString(AudioSettingsKey, json);
            PlayerPrefs.Save();
#else
            // Non-WebGL file-based saving
            var path = Path.Combine(PathsHelper.DataPath, PathsHelper.AudioSettingsDataJson);
            var json = JsonConvert.SerializeObject(_audioSettingsData);
            File.WriteAllText(path, json);
#endif
        }
    }
}
