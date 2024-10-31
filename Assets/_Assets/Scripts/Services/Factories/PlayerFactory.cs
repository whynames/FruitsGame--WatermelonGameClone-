using _Assets.Scripts.Misc;
using _Assets.Scripts.Services.StateMachine;
using _Assets.Scripts.Sprites;
using Cysharp.Threading.Tasks;
using UnityEngine;
using VContainer;
using VContainer.Unity;

namespace _Assets.Scripts.Services.Factories
{
    public class PlayerFactory : MonoBehaviour
    {
        [SerializeField] private GameObject playerPrefab;
        [SerializeField] private Transform spawnPoint;
        [Inject] private IObjectResolver _objectResolver;
        [Inject] private ResetService _resetService;
        [Inject] private SpriteServiceSettings _spriteServiceSettings;


        public async UniTask<GameObject> Create()
        {
            var sprite = _spriteServiceSettings.PlayerSprite;
            var player = _objectResolver.Instantiate(playerPrefab, spawnPoint.position, Quaternion.identity);
            player.GetComponentInChildren<SpriteRenderer>().sprite = sprite;
            _resetService.SetPlayer(player);
            return player;
        }
    }
}