using _Assets.Scripts.Services.StateMachine;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using VContainer;

namespace _Assets.Scripts.Services.UIs.InGame
{
    public class InGamePause : MonoBehaviour
    {
        [SerializeField] private Button pause;
        [Inject] private GameStateMachine _gameStateMachine;
        [Inject] private PlayerInput _playerInput;

        private void Awake() => pause.onClick.AddListener(Pause);

        private void Start() => _playerInput.OnPause += PauseInputCallback;

        private void PauseInputCallback(InputAction.CallbackContext context) => Pause();

        private void Pause() => _gameStateMachine.SwitchState(GameStateType.GamePause).Forget();

        private void OnDestroy()
        {
            pause.onClick.RemoveAllListeners();
            _playerInput.OnPause -= PauseInputCallback;
        }
    }
}