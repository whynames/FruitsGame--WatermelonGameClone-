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
        private readonly ScoreService _scoreService;
        private List<PlayerData> _gameDatas = new(5);
        public List<PlayerData> GameDatas => _gameDatas;

        public PlayerDataLoaderJson(ScoreService scoreService) => _scoreService = scoreService;

        public void SaveData()
        {
            var data = new PlayerData(_scoreService.Score);
            _gameDatas.Add(data);

            _gameDatas = _gameDatas.OrderByDescending(gameData => gameData).ToList();

            var dataToSave = new List<PlayerData>(5);

            for (int i = 0; i < _gameDatas.Count; i++)
            {
                dataToSave.Add(_gameDatas[i]);
            }

            var json = JsonConvert.SerializeObject(dataToSave);

            var path = Path.Combine(PathsHelper.DataPath, PathsHelper.PlayerDataJson);

            if (!Directory.Exists(PathsHelper.DataPath))
            {
                Directory.CreateDirectory(PathsHelper.DataPath);
            }

            using (StreamWriter streamWriter = new StreamWriter(path))
            {
                streamWriter.Write(json);
            }
        }

        public void LoadData()
        {
            var path = Path.Combine(PathsHelper.DataPath, PathsHelper.PlayerDataJson);

            if (!File.Exists(path))
            {
                Debug.LogWarning("Score data not found");
                return;
            }

            using (StreamReader reader = new StreamReader(Path.Combine(PathsHelper.DataPath, PathsHelper.PlayerDataJson)))
            {
                var json = reader.ReadToEnd();
                var gameDatas = JsonConvert.DeserializeObject<List<PlayerData>>(json);
                _gameDatas = gameDatas;
            }
        }
    }
}