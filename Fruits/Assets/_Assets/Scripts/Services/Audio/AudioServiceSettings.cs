using UnityEngine;

namespace _Assets.Scripts.Services.Audio
{
    [CreateAssetMenu(fileName = "AudioServiceSettings", menuName = "Configs/AudioServiceSettings", order = 0)]
    public class AudioServiceSettings : ScriptableObject
    {
        [field: SerializeField]
        public AudioClip CollisionClip { get; set; }
        [field: SerializeField]
        public AudioClip WinClip { get; set; }

        [field: SerializeField]
        public AudioClip[] MergeClips { get; set; }

        [field: SerializeField]
        public AudioClip[] MergeSongs { get; set; }
    }
}
