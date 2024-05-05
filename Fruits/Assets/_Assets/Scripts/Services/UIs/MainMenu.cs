using _Assets.Scripts.Services.StateMachine;
using _Assets.Scripts.Services.UIs.StateMachine;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using VContainer;

namespace _Assets.Scripts.Services.UIs
{
    public class MainMenu : MonoBehaviour
    {
        [SerializeField] private Button continueButton;
        [SerializeField] private Button playButton;

        [SerializeField] private Button settingsButton;

        [SerializeField] private Image background;
        [Inject] private GameStateMachine _gameStateMachine;
        [Inject] private UIStateMachine _uiStateMachine;
        [Inject] private ResumeGameService _resumeGameService;
        [Inject] private PlayerInput _playerInput;

        public void Init(Sprite sprite) => background.sprite = sprite;

        private void Awake()
        {
            playButton.onClick.AddListener(Play);
            // modsButton.onClick.AddListener(Mods);
            settingsButton.onClick.AddListener(Settings);
            // quitButton.onClick.AddListener(Quit);
        }

        private void Start()
        {
            _playerInput.OnDeviceChanged += SelectUIElement;

            var hasValidContinueData = _resumeGameService.HasData;

            if (hasValidContinueData)
            {
                if (_resumeGameService.FirstTimeOpened)
                {
                    Resume();
                    _resumeGameService.FirstTimeOpened = false;
                }
                var playButtonNavigation = playButton.navigation;
                playButtonNavigation.selectOnUp = settingsButton;
                playButtonNavigation.selectOnDown = settingsButton;
                playButton.navigation = playButtonNavigation;


            }

            // continueButton.gameObject.SetActive(hasValidContinueData);

            SelectFirstButton();
        }

        private void SelectFirstButton()
        {
            var hasValidContinueData = _resumeGameService.HasData;

            if (hasValidContinueData)
            {
                EventSystem.current.SetSelectedGameObject(continueButton.gameObject);
            }
            else
            {
                EventSystem.current.SetSelectedGameObject(playButton.gameObject);
            }
        }

        private void SelectUIElement(InputDevice device)
        {
            if (device.name == "Gamepad")
            {
                SelectFirstButton();
            }
        }

        private void Resume() => _gameStateMachine.SwitchState(GameStateType.ContinueGame).Forget();

        private void Play() => _gameStateMachine.SwitchState(GameStateType.Game).Forget();

        private void Mods() => _uiStateMachine.SwitchStateAndExitFromAllPrevious(UIStateType.Mods).Forget();

        private void Settings() => _uiStateMachine.SwitchState(UIStateType.Settings).Forget();

        private void Quit() => Application.Quit();
    }
}