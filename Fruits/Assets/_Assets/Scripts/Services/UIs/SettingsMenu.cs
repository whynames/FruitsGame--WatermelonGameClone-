using _Assets.Scripts.Services.Audio;
using _Assets.Scripts.Services.UIs.StateMachine;
using _Assets.Scripts.Services.Vibrations;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;
using VContainer;

namespace _Assets.Scripts.Services.UIs
{
    //SettingsMenu and InGamePauseMenu are similar
    //Could create an abstract base class for them
    public class SettingsMenu : MonoBehaviour
    {
        [SerializeField] private Slider soundSlider, musicSlider;
        [SerializeField] private Button vibrationButton;
        [SerializeField] private Button backButton;
        [SerializeField] private Image background;
        [Inject] private UIStateMachine _uiStateMachine;
        [Inject] private IAudioSettingsLoader _audioSettingsLoader;
        [Inject] private AudioService _audioService;
        [Inject] private IVibrationSettingLoader _vibrationSettingLoader;

        public void Init(Sprite sprite) => background.sprite = sprite;

        private void Awake()
        {
            soundSlider.onValueChanged.AddListener(ChangeSoundVolume);
            musicSlider.onValueChanged.AddListener(ChangeMusicVolume);
            vibrationButton.onClick.AddListener(ToggleVibration);
            backButton.onClick.AddListener(Back);
        }

        private void Start()
        {
            soundSlider.value = _audioSettingsLoader.AudioData.VFXVolume;
            musicSlider.value = _audioSettingsLoader.AudioData.MusicVolume;
            ChangeButton();
        }

        private void ChangeSoundVolume(float volume) => _audioService.ChangeSoundVolume(volume);

        private void ChangeMusicVolume(float volume) => _audioService.ChangeMusicVolume(volume);

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

        private void Back() => _uiStateMachine.SwitchToPreviousState().Forget();

        private void OnDestroy()
        {
            soundSlider.onValueChanged.RemoveAllListeners();
            musicSlider.onValueChanged.RemoveAllListeners();
            vibrationButton.onClick.RemoveAllListeners();
            backButton.onClick.RemoveAllListeners();
        }
    }
}