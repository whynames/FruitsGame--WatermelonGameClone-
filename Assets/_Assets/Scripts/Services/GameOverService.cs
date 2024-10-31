using System;
using _Assets.Scripts.Services.StateMachine;
using Cysharp.Threading.Tasks;
using VContainer.Unity;

namespace _Assets.Scripts.Services
{
    public class GameOverService : IInitializable, IDisposable
    {
        private readonly GameOverTimer _gameOverTimer;
        private readonly GameStateMachine _gameStateMachine;

        public GameOverService(GameOverTimer gameOverTimer, GameStateMachine gameStateMachine)
        {
            _gameOverTimer = gameOverTimer;
            _gameStateMachine = gameStateMachine;
        }

        public void Initialize() => _gameOverTimer.OnTimerEnded += GameOver;

        private void GameOver() => _gameStateMachine.SwitchState(GameStateType.GameOver).Forget();

        public void Dispose() => _gameOverTimer.OnTimerEnded -= GameOver;
    }
}