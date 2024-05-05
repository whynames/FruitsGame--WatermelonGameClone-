using _Assets.Scripts.Gameplay;
using _Assets.Scripts.Services.Factories;
using _Assets.Scripts.Services.UIs.StateMachine;
using Cysharp.Threading.Tasks;

namespace _Assets.Scripts.Services.StateMachine.States
{
    public class ResumeGameState : IGameState
    {
        private readonly UIStateMachine _uiStateMachine;
        private readonly ContainerFactory _containerFactory;
        private readonly PlayerFactory _playerFactory;
        private readonly PlayerInput _playerInput;
        private readonly ResumeGameService _resumeGameService;

        public ResumeGameState(UIStateMachine uiStateMachine, ContainerFactory containerFactory, PlayerFactory playerFactory, PlayerInput playerInput, ResumeGameService resumeGameService)
        {
            _uiStateMachine = uiStateMachine;
            _containerFactory = containerFactory;
            _playerFactory = playerFactory;
            _playerInput = playerInput;
            _resumeGameService = resumeGameService;
        }

        public async UniTask Enter()
        {
            await _uiStateMachine.SwitchState(UIStateType.Loading);
            _resumeGameService.Continue();
            _containerFactory.Create();
            var player = await _playerFactory.Create();
            await _uiStateMachine.SwitchState(UIStateType.Game);
            _resumeGameService.UpdateScore();
            player.GetComponent<PlayerDrop>().SpawnContinue();
            _playerInput.Enable();
        }

        public void Exit()
        {
        }
    }
}