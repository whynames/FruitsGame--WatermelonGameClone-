using _Assets.Scripts.Services.UIs.StateMachine;
using Cysharp.Threading.Tasks;

namespace _Assets.Scripts.Services.StateMachine.States
{
    public class GamePauseState : IGameState
    {
        private readonly PlayerInput _playerInput;
        private readonly UIStateMachine _uiStateMachine;

        public GamePauseState(PlayerInput playerInput, UIStateMachine uiStateMachine)
        {
            _playerInput = playerInput;
            _uiStateMachine = uiStateMachine;
        }

        public UniTask Enter()
        {
            _uiStateMachine.SwitchStateWithoutExitFromPrevious(UIStateType.Pause).Forget();
            _playerInput.Disable();
            return UniTask.CompletedTask;
        }

        public void Exit()
        {
        }
    }
}