using _Assets.Scripts.Misc;
using _Assets.Scripts.Services.StateMachine;
using _Assets.Scripts.Sprites;
using Cysharp.Threading.Tasks;
using UnityEngine;
using VContainer;
using VContainer.Unity;

namespace _Assets.Scripts.Services.Factories
{
    public class ContainerFactory : MonoBehaviour
    {
        [SerializeField] private new Camera camera;
        [SerializeField] private GameObject containerPrefab;
        [SerializeField] private Transform spawnPoint;
        [Inject] private IObjectResolver _objectResolver;
        [Inject] private ResetService _resetService;
        [Inject] private SpriteServiceSettings _spriteServiceSettings;

        public void Create()
        {
            var container = _objectResolver.Instantiate(containerPrefab, spawnPoint.position, Quaternion.identity);

            var sprite = _spriteServiceSettings.FieldSprite;
            container.GetComponentInChildren<SpriteRenderer>().sprite = sprite;

            float cameraHeight = camera.orthographicSize * 2;
            float cameraWidth = cameraHeight * Screen.width / Screen.height;
            var scalingFactorX = cameraWidth / sprite.bounds.size.x;
            var scalingFactorY = cameraHeight / sprite.bounds.size.y;
            var totalScale = new Vector3(scalingFactorX, scalingFactorY, 1);

            // foreach (var item in container.GetComponentsInChildren<SpriteRenderer>())
            // {
            //     item.transform.localScale = totalScale;
            // }


            _resetService.SetContainer(container);
        }
    }
}