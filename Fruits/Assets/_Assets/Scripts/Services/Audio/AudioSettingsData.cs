namespace _Assets.Scripts.Services.Audio
{
    public struct AudioSettingsData
    {
        public float MusicVolume;
        public float VFXVolume;

        public AudioSettingsData(float vfxVolume, float musicVolume)
        {
            VFXVolume = vfxVolume;
            MusicVolume = musicVolume;
        }
    }
}