using System;
using UnityEngine;

namespace _Assets.Scripts.Services
{
    public class ScoreService
    {
        public event Action<int> OnScoreChanged;

        public int Score
        {
            get => _score;
            private set
            {
                _score = value;
                OnScoreChanged?.Invoke(_score);
            }
        }

        private int _score;

        public void AddScore(int score)
        {
            if (score <= 0)
            {
                Debug.LogError("SCORE SERVICE: Score must be positive");
                return;
            }

            Score += score;
        }

        public void ResetScore() => Score = 0;
    }
}