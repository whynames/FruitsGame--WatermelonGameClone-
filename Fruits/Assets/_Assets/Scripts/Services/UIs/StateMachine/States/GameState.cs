using Cysharp.Threading.Tasks;
using UnityEngine;

namespace _Assets.Scripts.Services.UIs.StateMachine.States
{
    public class GameState : IUIState
    {

        private readonly UIMenuFactory _uiFactory;
        private GameObject _ui;

        public GameState(UIMenuFactory uiFactory)
        {
            _uiFactory = uiFactory;
        }

        public async UniTask Enter()
        {
            if (_ui == null)
            {
                var ui = await _uiFactory.CreateInGameUI();
                _ui = ui.gameObject;
            }
        }

        public void Exit() => Object.Destroy(_ui);
    }
}