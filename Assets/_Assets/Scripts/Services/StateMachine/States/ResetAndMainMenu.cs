using _Assets.Scripts.Services.Audio;
using _Assets.Scripts.Services.UIs.StateMachine;
using Cysharp.Threading.Tasks;

namespace _Assets.Scripts.Services.StateMachine.States
{
    public class ResetAndMainMenu : IGameState
    {
        private readonly GameStateMachine _gameStateMachine;
        private readonly ResetService _resetService;
        private readonly UIStateMachine _uiStateMachine;
        private readonly AudioService _audioService;
        private readonly ResumeGameService _resumeGameService;

        public ResetAndMainMenu(GameStateMachine gameStateMachine, ResetService resetService, UIStateMachine uiStateMachine, AudioService audioService, ResumeGameService resumeGameService)
        {
            _gameStateMachine = gameStateMachine;
            _resetService = resetService;
            _uiStateMachine = uiStateMachine;
            _audioService = audioService;
            _resumeGameService = resumeGameService;
        }

        public async UniTask Enter()
        {
            _resetService.Reset();
            _audioService.StopMusic();
            _audioService.ResetIndex();
            await _resumeGameService.Save();
            await _uiStateMachine.SwitchStateAndExitFromAllPrevious(UIStateType.MainMenu);
        }

        public void Exit()
        {
        }
    }
}