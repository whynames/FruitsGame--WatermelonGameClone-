using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace _Assets.Scripts.Services.UIs.StateMachine
{
    public class UIStateMachine
    {
        private readonly Dictionary<UIStateType, IUIState> _states;
        private IUIState _currentUIState;
        private IUIState _previousUIState;
        private UIStateType _currentUIStateType;
        private UIStateType _previousUIStateType;

        private readonly Dictionary<UIStateType, IUIState> _notExitedStates = new();

        public UIStateMachine(UIStatesFactory uiStatesFactory)
        {
            _states = new Dictionary<UIStateType, IUIState>
            {
                { UIStateType.Loading, uiStatesFactory.CreateLoadingState(this) },
                { UIStateType.MainMenu, uiStatesFactory.CreateMainMenuState(this) },
                { UIStateType.Mods, uiStatesFactory.CreateModsState(this) },
                { UIStateType.Game, uiStatesFactory.CreateGameState(this) },
                { UIStateType.GameOver, uiStatesFactory.CreateGameOverState(this) },
                { UIStateType.Settings, uiStatesFactory.CreateSettingsState(this) },
                { UIStateType.Pause, uiStatesFactory.CreateGamePauseState(this) }
            };
        }

        public async UniTask SwitchState(UIStateType uiStateType, int switchDelayInMilliseconds = 0)
        {
            if (_currentUIStateType == uiStateType)
            {
                Debug.LogError($"Already in {_currentUIStateType} state");
                return;
            }

            await UniTask.Delay(switchDelayInMilliseconds);

            _previousUIStateType = _currentUIStateType;
            _previousUIState = _currentUIState;

            _currentUIState = _states[uiStateType];
            _currentUIStateType = uiStateType;
            _notExitedStates.Remove(_previousUIStateType);
            await _currentUIState.Enter();
            _previousUIState?.Exit();
        }

        public async UniTask SwitchStateAndExitFromAllPrevious(UIStateType uiStateType, int switchDelayInMilliseconds = 0)
        {
            await SwitchState(uiStateType, switchDelayInMilliseconds);

            foreach (var uiState in _notExitedStates.Values)
            {
                uiState.Exit();
            }

            _notExitedStates.Clear();
        }

        public async UniTask SwitchToPreviousState(int switchDelayInMilliseconds = 0)
        {
            await SwitchState(_previousUIStateType, switchDelayInMilliseconds);
        }

        public async UniTask SwitchStateWithoutExitFromPrevious(UIStateType uiStateType, int switchDelayInMilliseconds = 0)
        {
            if (_currentUIStateType == uiStateType)
            {
                Debug.LogError($"Already in {_currentUIStateType} state");
                return;
            }

            await UniTask.Delay(switchDelayInMilliseconds);

            _notExitedStates.TryAdd(uiStateType, _currentUIState);

            _previousUIStateType = _currentUIStateType;
            _previousUIState = _states[_previousUIStateType];
            
            _currentUIState = _states[uiStateType];
            _currentUIStateType = uiStateType;

            _notExitedStates.TryAdd(_previousUIStateType, _previousUIState);

            await _currentUIState.Enter();
        }
    }
}