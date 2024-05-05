using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;
using UnityEngine;

namespace _Assets.Scripts.Services.Datas.Player
{
    public class PlayerDataLoaderPlayerPrefs : IPlayerDataLoader
    {
        private readonly ScoreService _scoreService;
        private List<PlayerData> _gameDatas = new(5);
        private const string DataKeyBase = "Data";

        public List<PlayerData> GameDatas => _gameDatas;

        public PlayerDataLoaderPlayerPrefs(ScoreService scoreService) => _scoreService = scoreService;

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
            
            var dataJson = JsonConvert.SerializeObject(dataToSave);
            PlayerPrefs.SetString(DataKeyBase, dataJson);
            PlayerPrefs.Save();
        }

        public void LoadData()
        {
            var dataJson = PlayerPrefs.GetString(DataKeyBase);

            if (string.IsNullOrEmpty(dataJson) || dataJson == "{}")
            {
                Debug.LogWarning("Data not found");
                return;
            }

            var data = JsonConvert.DeserializeObject<List<PlayerData>>(dataJson);

            var dataSorted = data.OrderByDescending(gameData => gameData).ToArray();

            var length = _gameDatas.Capacity > dataSorted.Length ?  dataSorted.Length : _gameDatas.Capacity;

            for (int i = 0; i < length; i++)
            {
                _gameDatas.Add(dataSorted[i]);
            }
            
            dataJson = JsonConvert.SerializeObject(_gameDatas);
            PlayerPrefs.SetString(DataKeyBase, dataJson);
            PlayerPrefs.Save();
        }
    }
}