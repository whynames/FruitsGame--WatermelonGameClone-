using UnityEngine;
using UnityEngine.InputSystem;
using VContainer;
using PlayerInput = _Assets.Scripts.Services.PlayerInput;

namespace _Assets.Scripts.Gameplay
{
    public class PlayerMovement : MonoBehaviour
    {
        [SerializeField] private float horizontalLimit = 2.65f;
        [Inject] private PlayerInput _playerInput;

        private void LateUpdate()
        {
            if (!_playerInput.Enabled()) return;

            var input = Mathf.Clamp(_playerInput.MoveVector.x, -horizontalLimit, horizontalLimit);
            var newPosition = transform.position + new Vector3(input, 0, 0);

            
            //Probably, should be in the input class and
            //return a vector3 depending on the device being used
            //but I don't care
            if (_playerInput.LastUsedDevice.name == "Mouse")
            {
                input = Mathf.Clamp(Camera.main.ScreenToWorldPoint(Mouse.current.position.value).x, -horizontalLimit, horizontalLimit);
                newPosition = new Vector3(input, transform.position.y, transform.position.z);
            }
            else if (_playerInput.LastUsedDevice.name == "Touchscreen")
            {
                input = Mathf.Clamp(Camera.main.ScreenToWorldPoint(Touchscreen.current.primaryTouch.position.value).x, -horizontalLimit, horizontalLimit);
                newPosition = new Vector3(input, transform.position.y, transform.position.z);
            }


            var clamped = Mathf.Clamp(newPosition.x, -horizontalLimit, horizontalLimit);
            transform.position = new Vector3(clamped, transform.position.y, transform.position.z);
        }
    }
}