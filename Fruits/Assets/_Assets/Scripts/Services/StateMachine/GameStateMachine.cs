using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace _Assets.Scripts.Services.StateMachine
{
    public class GameStateMachine
    {
        private readonly Dictionary<GameStateType, IGameState> _states;
        private IGameState _currentGameState;
        private GameStateType _currentGameStateType;

        public GameStateMachine(GameStatesFactory gameStatesFactory)
        {
            _states = new Dictionary<GameStateType, IGameState>
            {
                { GameStateType.LoadSavedData, gameStatesFactory.CreateLoadSaveDataState(this) },
                { GameStateType.Init, gameStatesFactory.CreateInitState(this) },
                { GameStateType.Game, gameStatesFactory.CreateGameState(this) },
                { GameStateType.GameOver, gameStatesFactory.CreateGameOverState(this) },
                { GameStateType.SaveData, gameStatesFactory.CreateSaveDataState(this) },
                { GameStateType.GameOverAndMainMenu, gameStatesFactory.CreateGameOverAndMainMenuState(this) },
                { GameStateType.ResetAndRetry, gameStatesFactory.CreateResetAndRetryState(this) },
                { GameStateType.ResetAndMainMenu, gameStatesFactory.CreateResetAndMainMenuState(this) },
                { GameStateType.GamePause, gameStatesFactory.CreateGamePauseState(this) },
                { GameStateType.GameResume, gameStatesFactory.CreateGameResumeState(this) },
                { GameStateType.ContinueGame, gameStatesFactory.CreateContinueGameState(this) }
            };
        }

        public async UniTask SwitchState(GameStateType gameStateType)
        {
            if (_currentGameStateType == gameStateType)
            {
                Debug.LogError($"Already in {_currentGameStateType} state");
                return;
            }

            _currentGameState?.Exit();
            _currentGameState = _states[gameStateType];
            _currentGameStateType = gameStateType;
            await _currentGameState.Enter();
        }
    }
}