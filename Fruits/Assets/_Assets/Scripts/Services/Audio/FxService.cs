using System.IO;
using System.Threading;
using _Assets.Scripts.Services.Datas.GameConfigs;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.Networking;
using VContainer;

namespace _Assets.Scripts.Services.Effects
{
    public class FxService : MonoBehaviour
    {


        [SerializeField] private FxServiceSettings fxSettingsLoader;


        public void PlayDestroy(Vector3 pos)
        {

            var particleObject = fxSettingsLoader.DestroyParticle;
            //I think object pooling is not needed here to keep the code concise, particle system will be destroyed as soon as it finishes playing.
            ParticleSystem particleInstance = GameObject.Instantiate(particleObject, pos, Quaternion.identity);
            particleInstance.Play();

        }
        public void PlayMerge(int index, Vector3 pos, int score)
        {
            var indexLocal = 0;
            if (index <= fxSettingsLoader.ParticleSystems.Length - 1)
            {
                indexLocal = index; // Adjust index to zero-based
            }
            else
            {
                // Return the last element of the array if index exceeds its length
                indexLocal = fxSettingsLoader.ParticleSystems.Length - 1;
            }
            var particleObject = fxSettingsLoader.ParticleSystems[indexLocal];

            //I think object pooling is not needed here to keep the code concise, particle system will be destroyed as soon as it finishes playing.
            ParticleSystem particleInstance = GameObject.Instantiate(particleObject, pos, Quaternion.identity);
            particleInstance.Play();

        }



    }
}