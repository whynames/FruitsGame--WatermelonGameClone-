using _Assets.Scripts.Services.Audio;
using _Assets.Scripts.Services.StateMachine;
using _Assets.Scripts.Services.Vibrations;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using VContainer;

namespace _Assets.Scripts.Services.UIs.InGame
{
    public class InGamePauseMenu : MonoBehaviour
    {
        //InGamePauseMenu and SettingsMenu are similar
        //Could create an abstract base class for them
        [SerializeField] private Slider soundSlider, musicSlider;
        [SerializeField] private Button backButton, mainMenuButton;
        [SerializeField] private Button vibrationButton;
        [Inject] private GameStateMachine _gameStateMachine;
        [Inject] private IAudioSettingsLoader _audioSettingsLoader;
        [Inject] private AudioService _audioService;
        [Inject] private PlayerInput _playerInput;
        [Inject] private IVibrationSettingLoader _vibrationSettingLoader;

        private void Awake()
        {
            mainMenuButton.onClick.AddListener(MainMenu);
            backButton.onClick.AddListener(Back);
            vibrationButton.onClick.AddListener(ToggleVibration);
        }

        private void Start()
        {
            ChangeButton();
            _playerInput.OnPause += Resume;
            soundSlider.value = _audioSettingsLoader.AudioData.VFXVolume;
            musicSlider.value = _audioSettingsLoader.AudioData.MusicVolume;
            soundSlider.onValueChanged.AddListener(ChangeSoundVolume);
            musicSlider.onValueChanged.AddListener(ChangeMusicVolume);
        }

        private void ToggleVibration()
        {
            _vibrationSettingLoader.Toggle(!_vibrationSettingLoader.VibrationSettingsData.Enabled);
            ChangeButton();
        }

        private void ChangeButton()
        {
            if (_vibrationSettingLoader.VibrationSettingsData.Enabled)
            {
                var colorBlock = vibrationButton.GetComponent<Button>().colors;
                colorBlock.normalColor = Color.green;
                vibrationButton.GetComponent<Button>().colors = colorBlock;
            }
            else
            {
                var colorBlock = vibrationButton.GetComponent<Button>().colors;
                colorBlock.normalColor = Color.red;
                vibrationButton.GetComponent<Button>().colors = colorBlock;
            }
        }

        private void Resume(InputAction.CallbackContext callback) => Back();

        private void MainMenu() => _gameStateMachine.SwitchState(GameStateType.ResetAndMainMenu).Forget();

        private void ChangeSoundVolume(float volume) => _audioService.ChangeSoundVolume(volume);

        private void ChangeMusicVolume(float volume) => _audioService.ChangeMusicVolume(volume);

        private void Back() => _gameStateMachine.SwitchState(GameStateType.GameResume).Forget();

        private void OnDestroy()
        {
            soundSlider.onValueChanged.RemoveAllListeners();
            musicSlider.onValueChanged.RemoveAllListeners();
            backButton.onClick.RemoveAllListeners();
            _playerInput.OnPause -= Resume;
        }
    }
}