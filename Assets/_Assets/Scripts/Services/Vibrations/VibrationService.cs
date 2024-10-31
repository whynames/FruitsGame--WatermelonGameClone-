namespace _Assets.Scripts.Services.Vibrations
{
    public class VibrationService
    {
        private readonly IVibrationSettingLoader _vibrationSettingLoader;
        private readonly GamepadRumbleService _gamepadRumbleService;

        private VibrationService(IVibrationSettingLoader vibrationSettingLoader, GamepadRumbleService gamepadRumbleService)
        {
            _vibrationSettingLoader = vibrationSettingLoader;
            _gamepadRumbleService = gamepadRumbleService;
        }

        public void Init()
        {
#if UNITY_ANDROID
            Vibration.Init();
#endif
        }

        public void Vibrate()
        {
#if UNITY_ANDROID
            if (_vibrationSettingLoader.VibrationSettingsData.Enabled)
            {
                Vibration.Vibrate();
            }
#endif
            if (_vibrationSettingLoader.VibrationSettingsData.Enabled)
            {
                _gamepadRumbleService.RumblePulse(0, 5, 0.25f, 0.5f);
            }
        }
    }
}