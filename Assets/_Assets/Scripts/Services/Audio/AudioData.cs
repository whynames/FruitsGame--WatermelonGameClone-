namespace _Assets.Scripts.Services.Audio
{
    public struct AudioData
    {
        public string Path;
        public float Volume;

        public AudioData(string path, float volume)
        {
            Path = path;
            Volume = volume;
        }
    }
}