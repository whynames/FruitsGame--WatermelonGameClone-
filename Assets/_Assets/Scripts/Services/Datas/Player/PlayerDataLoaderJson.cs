using System.Collections.Generic;
using System.IO;
using System.Linq;
using _Assets.Scripts.Misc;
using Newtonsoft.Json;
using UnityEngine;

namespace _Assets.Scripts.Services.Datas.Player
{
    public class PlayerDataLoaderJson : IPlayerDataLoader
    {
        private const string PlayerDataKey = "PlayerData";
        private readonly ScoreService _scoreService;
        private List<PlayerData> _gameDatas = new(5);
        public List<PlayerData> GameDatas => _gameDatas;

        public PlayerDataLoaderJson(ScoreService scoreService) => _scoreService = scoreService;

        public void SaveData()
        {
            var data = new PlayerData(_scoreService.Score);
            _gameDatas.Add(data);

            _gameDatas = _gameDatas.OrderByDescending(gameData => gameData).ToList();

            var dataToSave = _gameDatas.Take(5).ToList();
            var json = JsonConvert.SerializeObject(dataToSave);

#if UNITY_WEBGL
            // WebGL-specific saving to PlayerPrefs
            PlayerPrefs.SetString(PlayerDataKey, json);
            PlayerPrefs.Save();
#else
            // Non-WebGL file-based saving
            var path = Path.Combine(PathsHelper.DataPath, PathsHelper.PlayerDataJson);

            if (!Directory.Exists(PathsHelper.DataPath))
            {
                Directory.CreateDirectory(PathsHelper.DataPath);
            }

            using (StreamWriter streamWriter = new StreamWriter(path))
            {
                streamWriter.Write(json);
            }
#endif
        }

        public void LoadData()
        {
#if UNITY_WEBGL
            // WebGL-specific loading from PlayerPrefs
            if (PlayerPrefs.HasKey(PlayerDataKey))
            {
                var json = PlayerPrefs.GetString(PlayerDataKey);
                _gameDatas = JsonConvert.DeserializeObject<List<PlayerData>>(json);
            }
            else
            {
                Debug.LogWarning("Player data not found in PlayerPrefs.");
            }
#else
            // Non-WebGL file-based loading
            var path = Path.Combine(PathsHelper.DataPath, PathsHelper.PlayerDataJson);

            if (!File.Exists(path))
            {
                Debug.LogWarning("Score data not found");
                return;
            }

            using (StreamReader reader = new StreamReader(path))
            {
                var json = reader.ReadToEnd();
                _gameDatas = JsonConvert.DeserializeObject<List<PlayerData>>(json);
            }
#endif
        }
    }
}
