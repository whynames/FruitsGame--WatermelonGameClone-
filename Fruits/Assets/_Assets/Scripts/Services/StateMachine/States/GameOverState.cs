using _Assets.Scripts.Services.UIs.StateMachine;
using Cysharp.Threading.Tasks;

namespace _Assets.Scripts.Services.StateMachine.States
{
    public class GameOverState : IGameState
    {
        private readonly GameStateMachine _gameStateMachine;
        private readonly UIStateMachine _uiStateMachine;
        private readonly PlayerInput _playerInput;
        private readonly ResumeGameService _resumeGameService;

        public GameOverState(GameStateMachine gameStateMachine, UIStateMachine uiStateMachine, PlayerInput playerInput, ResumeGameService resumeGameService)
        {
            _gameStateMachine = gameStateMachine;
            _uiStateMachine = uiStateMachine;
            _playerInput = playerInput;
            _resumeGameService = resumeGameService;
        }

        public async UniTask Enter()
        {
            _playerInput.Disable();
            await _gameStateMachine.SwitchState(GameStateType.SaveData);
            _resumeGameService.DeleteContinueData();
            await _uiStateMachine.SwitchStateWithoutExitFromPrevious(UIStateType.GameOver);
        }

        public void Exit()
        {
        }
    }
}