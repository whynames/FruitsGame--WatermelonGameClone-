using _Assets.Scripts.Services.StateMachine;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;
using VContainer;

namespace _Assets.Scripts.Services.UIs
{
    public class GameOverMenu : MonoBehaviour
    {
        [SerializeField] private Button mainMenu;
        [SerializeField] private Button restart;
        [Inject] private GameStateMachine _stateMachine;

        private void Awake()
        {
            mainMenu.onClick.AddListener(ShowMainMenu);
            restart.onClick.AddListener(Restart);
        }

        private void ShowMainMenu() => _stateMachine.SwitchState(GameStateType.GameOverAndMainMenu).Forget();

        private void Restart() => _stateMachine.SwitchState(GameStateType.ResetAndRetry).Forget();
    }
}