using Cysharp.Threading.Tasks;

namespace _Assets.Scripts.Services.UIs.StateMachine.States
{
    public class LoadingState : IUIState
    {
        private readonly UIMenuFactory _uiFactory;
        private LoadingCurtain _ui;

        public LoadingState(UIMenuFactory uiFactory) => _uiFactory = uiFactory;

        public async UniTask Enter() => _ui = await _uiFactory.CreateLoadingCurtain();

        public void Exit() => _ui.Hide();
    }
}