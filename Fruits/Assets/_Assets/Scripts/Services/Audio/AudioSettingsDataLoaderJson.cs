using System.IO;
using _Assets.Scripts.Misc;
using Cysharp.Threading.Tasks;
using Newtonsoft.Json;
using UnityEngine;

namespace _Assets.Scripts.Services.Audio
{
    public class AudioSettingsDataLoaderJson : IAudioSettingsLoader
    {
        private AudioSettingsData _audioSettingsData;
        public AudioSettingsData AudioData => _audioSettingsData;
        public void ChangeSoundVolume(float volume) => _audioSettingsData.VFXVolume = volume;

        public void ChangeMusicVolume(float volume) => _audioSettingsData.MusicVolume = volume;

        public async UniTask Load()
        {
            var dataFolderInfo = new DirectoryInfo(PathsHelper.DataPath);
            
            if(!File.Exists(Path.Combine(PathsHelper.DataPath, PathsHelper.AudioSettingsDataJson)))
            {
                _audioSettingsData = new AudioSettingsData(.5f, .5f);
                Debug.LogWarning("Audio settings not found");
                return;
            }

            foreach (var fileInfo in dataFolderInfo.GetFiles(PathsHelper.AudioSettingsDataJson))
            {
                StreamReader reader = new StreamReader(fileInfo.FullName);
                var json = await reader.ReadToEndAsync();
                _audioSettingsData = JsonConvert.DeserializeObject<AudioSettingsData>(json);
                reader.Close();
                reader.Dispose();
            }
        }

        public void Save()
        {
            var path = Path.Combine(PathsHelper.DataPath, PathsHelper.AudioSettingsDataJson);
            var json = JsonConvert.SerializeObject(_audioSettingsData);
            File.WriteAllText(path, json);
        }
    }
}