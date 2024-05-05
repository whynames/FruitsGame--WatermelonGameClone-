using _Assets.Scripts.Services.Datas.Mods;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace _Assets.Scripts.Services.UIs.StateMachine.States
{
    public class MainMenuState : IUIState
    {
        private readonly UIMenuFactory _uiFactory;
        private readonly IModDataLoader _modDataLoader;
        private GameObject _ui;

        public MainMenuState(UIMenuFactory uiFactory, IModDataLoader modDataLoader)
        {
            _uiFactory = uiFactory;
            _modDataLoader = modDataLoader;
        }

        public async UniTask Enter()
        {
            _modDataLoader.Save();
            var ui = await _uiFactory.CreateMainMenuUI();
            _ui = ui.gameObject;
        }

        public void Exit() => Object.Destroy(_ui);
    }
}