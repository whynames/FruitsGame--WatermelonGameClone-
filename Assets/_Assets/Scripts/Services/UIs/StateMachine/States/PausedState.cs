using Cysharp.Threading.Tasks;
using UnityEngine;

namespace _Assets.Scripts.Services.UIs.StateMachine.States
{
    public class PausedState : IUIState
    {
        private readonly UIMenuFactory _uiFactory;
        private GameObject _ui;

        public PausedState(UIMenuFactory uiFactory) => _uiFactory = uiFactory;

        public UniTask Enter()
        {
            _ui = _uiFactory.CreatePauseUI().gameObject;
            return UniTask.CompletedTask;
        }

        public void Exit() => Object.Destroy(_ui);
    }
}