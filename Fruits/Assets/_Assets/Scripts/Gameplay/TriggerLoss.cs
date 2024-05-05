using System.Collections.Generic;
using _Assets.Scripts.Services;
using _Assets.Scripts.Services.Datas.GameConfigs;
using UnityEngine;
using VContainer;

namespace _Assets.Scripts.Gameplay
{
    public class TriggerLoss : MonoBehaviour
    {
        private float _time;
        private List<Fruit> _collidedFruits = new(10);
        private bool _ended;
        [Inject] private GameOverTimer _gameOverTimer;
        [Inject] private IConfigLoader _configLoader;

        private void Start() => _gameOverTimer.OnTimerEnded += TimerEnded;

        private void TimerEnded() => _ended = true;

        private void Update()
        {
            if (_collidedFruits.Count > 0 && !_ended)
            {
                _time += Time.deltaTime;

                if (_time >= _configLoader.CurrentConfig.TimeBeforeTimerTrigger)
                {
                    _gameOverTimer.StartTimer();
                }
            }
            else
            {
                _gameOverTimer.StopTimer();
                _time = 0;
            }
        }

        private void OnTriggerStay2D(Collider2D other)
        {
            if (other.TryGetComponent(out Fruit fruit))
            {
                if (!_collidedFruits.Contains(fruit))
                {
                    if (fruit.HasLanded)
                    {
                        _collidedFruits.Add(fruit);
                    }
                }
            }
        }

        private void OnTriggerExit2D(Collider2D other)
        {
            if (other.TryGetComponent(out Fruit fruit))
            {
                if (_collidedFruits.Contains(fruit))
                {
                    _collidedFruits.Remove(fruit);
                }
            }
        }

        private void OnDestroy() => _gameOverTimer.OnTimerEnded -= TimerEnded;
    }
}