using _Assets.Scripts.Services.UIs.Mods;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace _Assets.Scripts.Services.UIs.StateMachine.States
{
    public class ModsState : IUIState
    {
        private readonly UIMenuFactory _uiFactory;
        private ModsMenu _ui;

        public ModsState(UIMenuFactory uiFactory) => _uiFactory = uiFactory;

        public UniTask Enter()
        {
            _ui = _uiFactory.CreateModUI();
            return UniTask.CompletedTask;
        }

        public void Exit() => Object.Destroy(_ui.gameObject);
    }
}