using UnityEngine;

namespace _Assets.Scripts.Services.Effects
{
    [CreateAssetMenu(fileName = "FxServiceList", menuName = "Configs/FxService", order = 0)]
    public class FxServiceSettings : ScriptableObject
    {
        [field: SerializeField]
        public ParticleSystem[] ParticleSystems { get; set; }
        [field: SerializeField]
        public ParticleSystem DestroyParticle { get; set; }

    }
}