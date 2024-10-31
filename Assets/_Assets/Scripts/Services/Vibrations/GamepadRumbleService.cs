using System;
using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem;

namespace _Assets.Scripts.Services.Vibrations
{
    public class GamepadRumbleService : MonoBehaviour
    {
        private float _rumbleDuration;
        private float _pulseDuration;
        private float _lowAmplitude;
        private float _highAmplitude;
        private float _pulseTime;
        private bool _isMotorActive;

        public void RumblePulse(float low, float high, float pulseDuration, float rubleDuration)
        {
            if (Math.Abs(rubleDuration - pulseDuration) < 0.001f)
            {
                Debug.LogError("Detected endless rumble loop", this);
                return;
            }

            _lowAmplitude = low;
            _highAmplitude = high;
            _pulseTime = pulseDuration;
            _pulseDuration = Time.time + pulseDuration;
            _rumbleDuration = Time.time + rubleDuration;
            _isMotorActive = true;
            var gamepad = GetGamepad();
            gamepad?.SetMotorSpeeds(_lowAmplitude, _highAmplitude);
        }

        private void StopRumble()
        {
            var gamepad = GetGamepad();
            gamepad?.SetMotorSpeeds(0, 0);
        }

        private void Update()
        {
            if (Time.time > _rumbleDuration)
            {
                StopRumble();
                return;
            }

            var gamepad = GetGamepad();
            if (gamepad == null)
                return;

            if (Time.time > _pulseDuration)
            {
                _isMotorActive = !_isMotorActive;
                _pulseDuration = Time.time + _pulseTime;
                if (!_isMotorActive)
                {
                    gamepad.SetMotorSpeeds(0, 0);
                }
                else
                {
                    gamepad.SetMotorSpeeds(_lowAmplitude, _highAmplitude);
                }
            }
        }

        private void OnDestroy() => StopRumble();

        private Gamepad GetGamepad() =>
            Gamepad.all.FirstOrDefault(g => InputSystem.devices.Any(d => d.deviceId == g.deviceId));
    }
}