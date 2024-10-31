using System.IO;
using _Assets.Scripts.Misc;
using Cysharp.Threading.Tasks;
using Newtonsoft.Json;
using UnityEngine;

namespace _Assets.Scripts.Services.Vibrations
{
    public class VibrationSettingsDataLoaderJson : IVibrationSettingLoader
    {

#if !UNITY_WEBGL
        private VibrationSettingsData _vibrationSettingsData;
        public VibrationSettingsData VibrationSettingsData => _vibrationSettingsData;

        public async UniTask Load()
        {
            var dataFolderInfo = new DirectoryInfo(PathsHelper.DataPath);

            if (!File.Exists(Path.Combine(PathsHelper.DataPath, PathsHelper.VibrationSettingsDataJson)))
            {
                _vibrationSettingsData = new VibrationSettingsData(true);
                Debug.LogWarning("Vibration settings not found");
                return;
            }

            foreach (var fileInfo in dataFolderInfo.GetFiles(PathsHelper.VibrationSettingsDataJson))
            {
                StreamReader reader = new StreamReader(fileInfo.FullName);
                var json = await reader.ReadToEndAsync();
                _vibrationSettingsData = JsonConvert.DeserializeObject<VibrationSettingsData>(json);
                reader.Close();
                reader.Dispose();
            }
        }

        public void Save()
        {
            var path = Path.Combine(PathsHelper.DataPath, PathsHelper.VibrationSettingsDataJson);
            var json = JsonConvert.SerializeObject(_vibrationSettingsData);
            File.WriteAllText(path, json);
        }

        public void Toggle(bool enabled) => _vibrationSettingsData.Enabled = enabled;
#else

     private const string VibrationSettingsKey = "VibrationSettingsData";
        private VibrationSettingsData _vibrationSettingsData;
        public VibrationSettingsData VibrationSettingsData => _vibrationSettingsData;

        public async UniTask Load()
        {
            // Load data from PlayerPrefs if available
            if (PlayerPrefs.HasKey(VibrationSettingsKey))
            {
                var json = PlayerPrefs.GetString(VibrationSettingsKey);
                _vibrationSettingsData = JsonConvert.DeserializeObject<VibrationSettingsData>(json);
            }
            else
            {
                // If not found, set default values
                _vibrationSettingsData = new VibrationSettingsData(true);
                Debug.LogWarning("Vibration settings not found. Using default settings.");
            }
        }

        public void Save()
        {
            // Serialize data to JSON and store in PlayerPrefs
            var json = JsonConvert.SerializeObject(_vibrationSettingsData);
            PlayerPrefs.SetString(VibrationSettingsKey, json);
            PlayerPrefs.Save();
        }

        public void Toggle(bool enabled)
        {
            _vibrationSettingsData.Enabled = enabled;
            Save(); // Save the updated setting
        }
#endif
    }
}