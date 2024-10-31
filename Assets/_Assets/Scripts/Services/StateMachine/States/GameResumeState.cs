using _Assets.Scripts.Services.Audio;
using _Assets.Scripts.Services.UIs.StateMachine;
using Cysharp.Threading.Tasks;

namespace _Assets.Scripts.Services.StateMachine.States
{
    public class GameResumeState : IGameState
    {
        private readonly PlayerInput _playerInput;
        private readonly UIStateMachine _uiStateMachine;
        private readonly IAudioSettingsLoader _audioSettingsLoader;

        public GameResumeState(PlayerInput playerInput, UIStateMachine uiStateMachine, IAudioSettingsLoader audioSettingsLoader)
        {
            _playerInput = playerInput;
            _uiStateMachine = uiStateMachine;
            _audioSettingsLoader = audioSettingsLoader;
        }

        public async UniTask Enter()
        {
            _uiStateMachine.SwitchState(UIStateType.Game).Forget();
            await UniTask.DelayFrame(1);
            _playerInput.Enable();
        }

        public void Exit()
        {
            _audioSettingsLoader.Save();
        }
    }
}