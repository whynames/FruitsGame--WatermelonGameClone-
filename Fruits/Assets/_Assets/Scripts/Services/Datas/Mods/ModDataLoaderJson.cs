using System.IO;
using _Assets.Scripts.Misc;
using Cysharp.Threading.Tasks;
using Newtonsoft.Json;
using UnityEngine;

namespace _Assets.Scripts.Services.Datas.Mods
{
    public class ModDataLoaderJson : IModDataLoader
    {
        private ModData _modData = new("Default");
        public ModData ModData => _modData;
        public void SetModName(string modName)
        {
            _modData.SelectedModName = modName;
            Debug.LogError("CURRENT MOD NAME: " + _modData.SelectedModName);
        }

        public async UniTask Load()
        {
            var dataFolderInfo = new DirectoryInfo(PathsHelper.DataPath);

            if (!dataFolderInfo.Exists)
            {
                dataFolderInfo.Create();
                return;
            }
            
            foreach (var fileInfo in dataFolderInfo.GetFiles(PathsHelper.ModDataJson))
            {
                var reader = new StreamReader(fileInfo.FullName);
                var json = await reader.ReadToEndAsync();
                _modData = JsonConvert.DeserializeObject<ModData>(json);
                reader.Close();
                reader.Dispose();
            }
        }

        public void Save()
        {
            var path = Path.Combine(PathsHelper.DataPath, PathsHelper.ModDataJson);
            var json = JsonConvert.SerializeObject(_modData);
            File.WriteAllText(path, json);
        }
    }
}