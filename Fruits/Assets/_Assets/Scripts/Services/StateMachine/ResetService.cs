using System.Collections.Generic;
using _Assets.Scripts.Gameplay;
using UnityEngine;

namespace _Assets.Scripts.Services.StateMachine
{
    public class ResetService
    {
        private readonly List<Fruit> _fruits = new();
        private GameObject _fruitContainer;
        private GameObject _player;
        private readonly GameOverTimer _gameOverTimer;
        private readonly ScoreService _scoreService;

        private ResetService(GameOverTimer gameOverTimer, ScoreService scoreService)
        {
            _gameOverTimer = gameOverTimer;
            _scoreService = scoreService;
        }

        public void AddFruit(Fruit fruit) => _fruits.Add(fruit);
        public void RemoveFruit(Fruit fruit) => _fruits.Remove(fruit);
        public void SetContainer(GameObject fruitContainer) => _fruitContainer = fruitContainer;
        public void SetPlayer(GameObject player) => _player = player;

        public void Reset()
        {
            _gameOverTimer.StopTimer();
            _scoreService.ResetScore();

            var fruitsCount = _fruits.Count - 1;
            for (int i = fruitsCount; i >= 0; i--)
            {
                Object.Destroy(_fruits[i].gameObject);
            }

            _fruits.Clear();

            Object.Destroy(_fruitContainer);
            Object.Destroy(_player);
        }
    }
}