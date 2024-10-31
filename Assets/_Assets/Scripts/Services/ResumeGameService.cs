using System;
using System.Collections.Generic;
using System.IO;
using _Assets.Scripts.Gameplay;
using _Assets.Scripts.Misc;
using _Assets.Scripts.Services.Audio;
using _Assets.Scripts.Services.Factories;
using Cysharp.Threading.Tasks;
using Newtonsoft.Json;
using UnityEngine;

namespace _Assets.Scripts.Services
{
    public class ResumeGameService
    {
        private ContinueData _continueData;
        private readonly List<Fruit> _fruits = new();
        private readonly AudioService _audioService;
        private readonly FruitsFactory _fruitsFactory;
        private readonly RandomNumberGenerator _randomNumberGenerator;
        private readonly ScoreService _scoreService;

        private const string ContinueDataKey = "ContinueData";

        public bool HasData => _continueData != null;
        public bool FirstTimeOpened { get; set; }

        private ResumeGameService(AudioService audioService, FruitsFactory fruitsFactory,
            RandomNumberGenerator randomNumberGenerator, ScoreService scoreService)
        {
            _audioService = audioService;
            _fruitsFactory = fruitsFactory;
            _randomNumberGenerator = randomNumberGenerator;
            _scoreService = scoreService;
        }

        public async void Continue()
        {
            await Load();

            _audioService.PlaySongContinue(_continueData.SongIndex).Forget();

            Debug.LogError($"Song index: {_continueData.SongIndex}");

            for (int i = 0; i < _continueData.FruitsContinueData.Count; i++)
            {
                var position = new Vector3(_continueData.FruitsContinueData[i].PositionX,
                    _continueData.FruitsContinueData[i].PositionY, 0);
                _fruitsFactory.CreateContinue(_continueData.FruitsContinueData[i].Index, position);
            }

            _randomNumberGenerator.SetCurrent(_continueData.CurrentFruitIndex);
            _randomNumberGenerator.SetNext(_continueData.NextFruitIndex);
        }

        public void UpdateScore()
        {
            _scoreService.AddScore(_continueData.Score);
        }

        public async UniTask Load()
        {
#if UNITY_WEBGL
            // WebGL-specific loading from PlayerPrefs
            if (PlayerPrefs.HasKey(ContinueDataKey))
            {
                var json = PlayerPrefs.GetString(ContinueDataKey);
                _continueData = JsonConvert.DeserializeObject<ContinueData>(json);
            }
            else
            {
                Debug.LogWarning("Continue data not found.");
                _continueData = null;
            }
#else
            // Non-WebGL file-based loading
            var dataFolderInfo = new DirectoryInfo(PathsHelper.DataPath);

            if (!dataFolderInfo.Exists)
            {
                dataFolderInfo.Create();
                return;
            }

            foreach (var fileInfo in dataFolderInfo.GetFiles(PathsHelper.ContinueDataJson))
            {
                using (var reader = new StreamReader(fileInfo.FullName))
                {
                    try
                    {
                        var json = await reader.ReadToEndAsync();
                        _continueData = JsonConvert.DeserializeObject<ContinueData>(json);
                    }
                    catch (Exception e)
                    {
                        Debug.LogError(e);
                        throw;
                    }
                }
            }
#endif
        }

        public void AddFruit(Fruit fruit) => _fruits.Add(fruit);

        public void RemoveFruit(Fruit fruit) => _fruits.Remove(fruit);

        public async UniTask Save()
        {
            _continueData = new ContinueData(_audioService.LastSongIndex, new List<ContinueData.FruitContinueData>(), _randomNumberGenerator.Current, _randomNumberGenerator.Next, _scoreService.Score);

            _continueData.FruitsContinueData = new List<ContinueData.FruitContinueData>(_fruits.Count);
            for (int i = 0; i < _fruits.Count; i++)
            {
                var index = _fruits[i].Index;
                var position = _fruits[i].transform.position;
                _continueData.FruitsContinueData.Add(new ContinueData.FruitContinueData(index, position.x, position.y));
            }

            _continueData.SongIndex = _audioService.LastSongIndex;
            _continueData.CurrentFruitIndex = _randomNumberGenerator.Current;
            _continueData.NextFruitIndex = _randomNumberGenerator.Next;
            _continueData.Score = _scoreService.Score;

#if UNITY_WEBGL
            // WebGL-specific saving to PlayerPrefs
            var json = JsonConvert.SerializeObject(_continueData);
            PlayerPrefs.SetString(ContinueDataKey, json);
            PlayerPrefs.Save();
#else
            // Non-WebGL file-based saving
            var path = Path.Combine(PathsHelper.DataPath, PathsHelper.ContinueDataJson);
            var json = JsonConvert.SerializeObject(_continueData);
            await File.WriteAllTextAsync(path, json);
#endif
        }

        public void DeleteContinueData()
        {
            _continueData = null;

#if UNITY_WEBGL
            // WebGL-specific deletion from PlayerPrefs
            PlayerPrefs.DeleteKey(ContinueDataKey);
#else
            // Non-WebGL file-based deletion
            var path = Path.Combine(PathsHelper.DataPath, PathsHelper.ContinueDataJson);
            if (File.Exists(path))
            {
                File.Delete(path);
            }
#endif
        }
    }
}
