using Cysharp.Threading.Tasks;

namespace _Assets.Scripts.Services.StateMachine.States
{
    public class ResetAndRetry : IGameState
    {
        private readonly GameStateMachine _gameStateMachine;
        private readonly ResetService _resetService;

        public ResetAndRetry(GameStateMachine gameStateMachine, ResetService resetService)
        {
            _gameStateMachine = gameStateMachine;
            _resetService = resetService;
        }

        public async UniTask Enter()
        {
            _resetService.Reset();
            await _gameStateMachine.SwitchState(GameStateType.Game);
        }

        public void Exit()
        {
        }
    }
}