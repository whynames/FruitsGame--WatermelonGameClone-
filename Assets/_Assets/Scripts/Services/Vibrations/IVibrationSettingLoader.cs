using Cysharp.Threading.Tasks;

namespace _Assets.Scripts.Services.Vibrations
{
    public interface IVibrationSettingLoader
    {
        UniTask Load();
        void Save();
        void Toggle(bool enabled);
        VibrationSettingsData VibrationSettingsData { get; }
    }
}