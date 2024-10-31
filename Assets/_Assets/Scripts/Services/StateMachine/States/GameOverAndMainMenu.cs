using _Assets.Scripts.Services.Audio;
using _Assets.Scripts.Services.UIs.StateMachine;
using Cysharp.Threading.Tasks;

namespace _Assets.Scripts.Services.StateMachine.States
{
    public class GameOverAndMainMenu : IGameState
    {
        private readonly ResetService _resetService;
        private readonly UIStateMachine _uiStateMachine;
        private readonly AudioService _audioService;

        public GameOverAndMainMenu(ResetService resetService, UIStateMachine uiStateMachine, AudioService audioService)
        {
            _resetService = resetService;
            _uiStateMachine = uiStateMachine;
            _audioService = audioService;
        }
        
        public async UniTask Enter()
        {
            _resetService.Reset();
            _audioService.StopMusic();
            _audioService.ResetIndex();
            await _uiStateMachine.SwitchStateAndExitFromAllPrevious(UIStateType.MainMenu);
        }

        public void Exit()
        {
        }
    }
}