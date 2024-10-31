using System.IO;
using _Assets.Scripts.Misc;
using Cysharp.Threading.Tasks;
using Newtonsoft.Json;
using UnityEngine;

namespace _Assets.Scripts.Services.Datas.Mods
{
    public class ModDataLoaderJson : IModDataLoader
    {
        private const string ModDataKey = "ModData";
        private ModData _modData = new("Default");
        public ModData ModData => _modData;

        public void SetModName(string modName)
        {
            _modData.SelectedModName = modName;
            Debug.LogError("CURRENT MOD NAME: " + _modData.SelectedModName);
        }

        public async UniTask Load()
        {
#if UNITY_WEBGL
            // WebGL-specific loading from PlayerPrefs
            if (PlayerPrefs.HasKey(ModDataKey))
            {
                var json = PlayerPrefs.GetString(ModDataKey);
                _modData = JsonConvert.DeserializeObject<ModData>(json);
            }
            else
            {
                Debug.LogWarning("Mod data not found. Using default.");
            }
#else
            // Non-WebGL file-based loading
            var dataFolderInfo = new DirectoryInfo(PathsHelper.DataPath);

            if (!dataFolderInfo.Exists)
            {
                dataFolderInfo.Create();
                return;
            }

            foreach (var fileInfo in dataFolderInfo.GetFiles(PathsHelper.ModDataJson))
            {
                using (var reader = new StreamReader(fileInfo.FullName))
                {
                    var json = await reader.ReadToEndAsync();
                    _modData = JsonConvert.DeserializeObject<ModData>(json);
                }
            }
#endif
        }

        public void Save()
        {
#if UNITY_WEBGL
            // WebGL-specific saving to PlayerPrefs
            var json = JsonConvert.SerializeObject(_modData);
            PlayerPrefs.SetString(ModDataKey, json);
            PlayerPrefs.Save();
#else
            // Non-WebGL file-based saving
            var path = Path.Combine(PathsHelper.DataPath, PathsHelper.ModDataJson);
            var json = JsonConvert.SerializeObject(_modData);
            File.WriteAllText(path, json);
#endif
        }
    }
}
