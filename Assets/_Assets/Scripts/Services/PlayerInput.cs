using System;
using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.LowLevel;

namespace _Assets.Scripts.Services
{
    public class PlayerInput : MonoBehaviour
    {
        [SerializeField] private InputActionAsset controls;
        private bool _enabled;
        private InputAction _moveAction;
        public bool Enabled(int fingerId = -1)
        {
            bool enabled = _enabled && !EventSystem.current.IsPointerOverGameObject();
            #if UNITY_ANDROID
                enabled = _enabled && !EventSystem.current.IsPointerOverGameObject() && !EventSystem.current.IsPointerOverGameObject(fingerId);
            #endif
            return enabled;
        }

        private InputDevice _lastUsedDevice;
        public InputDevice LastUsedDevice => _lastUsedDevice;

        public event Action<InputDevice> OnDeviceChanged;
        public event Action<InputAction.CallbackContext> OnPause;
        public event Action<InputAction.CallbackContext> OnDrop;
        public Vector2 MoveVector => _moveAction.ReadValue<Vector2>();

        public void Init()
        {
            //Since it's a singleton and init is called only once the game starts, we can forget about unsubscribing.
            InputSystem.onEvent += OnInputSystemEvent;
            InputSystem.onDeviceChange += OnDeviceChange;
            controls.FindActionMap("Game").FindAction("Pause").performed += PauseInputCallback;
            controls.FindActionMap("Game").FindAction("Drop").performed += DropCallback;
            _moveAction = controls.FindActionMap("Game").FindAction("Move");
            controls.Enable();
        }

        private void DropCallback(InputAction.CallbackContext callback) => OnDrop?.Invoke(callback);

        private void PauseInputCallback(InputAction.CallbackContext callback) => OnPause?.Invoke(callback);

        public void Enable() => _enabled = true;

        public void Disable() => _enabled = false;

        private void OnDeviceChange(InputDevice device, InputDeviceChange change)
        {
            if (Equals(_lastUsedDevice, device))
                return;

            _lastUsedDevice = device;
            OnDeviceChanged?.Invoke(_lastUsedDevice);
        }


        private void OnInputSystemEvent(InputEventPtr eventPtr, InputDevice device)
        {
            if (_lastUsedDevice == device)
                return;

            // Some devices like to spam events like crazy.
            // Example: PS4 controller on PC keeps triggering events without meaningful change.
            var eventType = eventPtr.type;
            if (eventType == StateEvent.Type)
            {
                // Go through the changed controls in the event and look for ones actuated
                // above a magnitude of a little above zero.
                if (!eventPtr.EnumerateChangedControls(device: device, magnitudeThreshold: 0.0001f).Any())
                    return;
            }


            _lastUsedDevice = device;
            OnDeviceChanged?.Invoke(_lastUsedDevice);
        }
    }
}