using Cysharp.Threading.Tasks;
using UnityEngine;

namespace _Assets.Scripts.Services.UIs.StateMachine.States
{
    public class GameOverState : IUIState
    {
        private readonly UIMenuFactory _uiFactory;
        private GameObject _ui;

        public GameOverState(UIMenuFactory uiFactory) => _uiFactory = uiFactory;

        public UniTask Enter()
        {
            _ui = _uiFactory.CreateGameOverUI().gameObject;
            return UniTask.CompletedTask;
        }

        public void Exit() => Object.Destroy(_ui);
    }
}