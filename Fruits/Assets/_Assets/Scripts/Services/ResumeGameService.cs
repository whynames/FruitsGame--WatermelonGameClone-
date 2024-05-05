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
            var dataFolderInfo = new DirectoryInfo(PathsHelper.DataPath);

            if (!dataFolderInfo.Exists)
            {
                dataFolderInfo.Create();
                return;
            }

            foreach (var fileInfo in dataFolderInfo.GetFiles(PathsHelper.ContinueDataJson))
            {
                var reader = new StreamReader(fileInfo.FullName);

                try
                {
                    var json = await reader.ReadToEndAsync();
                    _continueData = JsonConvert.DeserializeObject<ContinueData>(json);
                    reader.Close();
                    reader.Dispose();
                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                    reader.Close();
                    reader.Dispose();
                    throw;
                }
            }
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

            var path = Path.Combine(PathsHelper.DataPath, PathsHelper.ContinueDataJson);
            var json = JsonConvert.SerializeObject(_continueData);
            await File.WriteAllTextAsync(path, json);
        }

        public void DeleteContinueData()
        {
            _continueData = null;
            var path = Path.Combine(PathsHelper.DataPath, PathsHelper.ContinueDataJson);
            File.Delete(path);
        }
    }
}