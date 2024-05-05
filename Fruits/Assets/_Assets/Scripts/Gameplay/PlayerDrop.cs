using System.Collections;
using _Assets.Scripts.Services.Factories;
using _Assets.Scripts.Services.UIs;
using UnityEngine;
using UnityEngine.InputSystem;
using VContainer;
using PlayerInput = _Assets.Scripts.Services.PlayerInput;

namespace _Assets.Scripts.Gameplay
{
    public class PlayerDrop : MonoBehaviour
    {
        [Inject] private FruitsFactory _fruitsFactory;
        [Inject] private FruitUIDataProvider _fruitUIDataProvider;
        [Inject] private PlayerInput _playerInput;

        [SerializeField]
        Vector3 offset;
        private Rigidbody2D _fruitRigidbody;
        private bool _canDrop = true;
        private readonly YieldInstruction _wait = new WaitForSeconds(1f);

        private void Start() => _playerInput.OnDrop += Drop;

        private void Drop(InputAction.CallbackContext callback)
        {
            if (_canDrop && _playerInput.Enabled())
            {
                Drop();
                StartCoroutine(Cooldown());
            }
        }

        public void SpawnFruit() => Spawn();

        public void SpawnContinue() => _fruitRigidbody = _fruitsFactory.CreatePlayerContinue(transform.position + offset, transform);

        private void Spawn()
        {
            _fruitRigidbody = _fruitsFactory.CreateToDrop(transform.position + offset, transform);
        }

        private void Drop()
        {
            _fruitRigidbody.transform.parent = null;
            _fruitRigidbody.isKinematic = false;
            _fruitRigidbody.GetComponent<Fruit>().Drop();
            _fruitRigidbody = null;
        }

        private IEnumerator Cooldown()
        {
            _canDrop = false;
            yield return _wait;
            Spawn();
            _canDrop = true;
        }

        private void OnDestroy() => _playerInput.OnDrop -= Drop;
    }
}