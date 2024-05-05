using _Assets.Scripts.Services.Audio;
using _Assets.Scripts.Services.Vibrations;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace _Assets.Scripts.Services.UIs.StateMachine.States
{
    public class SettingsState : IUIState
    {
        private readonly UIMenuFactory _uiFactory;
        private GameObject _ui;
        private readonly IAudioSettingsLoader _audioSettingsLoader;
        private readonly IVibrationSettingLoader _vibrationSettingLoader;

        public SettingsState(UIMenuFactory uiFactory, IAudioSettingsLoader audioSettingsLoader, IVibrationSettingLoader vibrationSettingLoader)
        {
            _uiFactory = uiFactory;
            _audioSettingsLoader = audioSettingsLoader;
            _vibrationSettingLoader = vibrationSettingLoader;
        }

        public UniTask Enter()
        {
            _ui = _uiFactory.CreateSettingsMenu().gameObject;
            return UniTask.CompletedTask;
        }

        public void Exit()
        {
            _audioSettingsLoader.Save();
            _vibrationSettingLoader.Save();
            Object.Destroy(_ui);
        }
    }
}