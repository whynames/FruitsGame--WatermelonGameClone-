using UnityEngine;

namespace _Assets.Scripts.Misc
{
    public class CameraScaler : MonoBehaviour
    {
        [SerializeField] private new Camera camera;

        [SerializeField] protected int targetWidth = 1080;
        [SerializeField] protected int targetHeight = 1920;

        [SerializeField] protected int dynamicMaxWidth = 1440;
        [SerializeField] protected int dynamicMaxHeight = 2560;

        [SerializeField] protected bool useDynamicWidth;
        [SerializeField] protected bool useDynamicHeight;

        private int _lastWidth;
        private int _lastHeight;
        private float _orthoSize;

        protected void Awake() => _orthoSize = camera.orthographicSize;

        protected void Update()
        {
#if !UNITY_ANDROID
            if (Screen.width != _lastWidth || Screen.height != _lastHeight)
            {
                UpdateCamSize();
                _lastWidth = Screen.width;
                _lastHeight = Screen.height;
            }
#endif
        }

        private void UpdateCamSize()
        {
            float targetAspect;
            var screenAspect = Screen.width / (float)Screen.height;
            var orthoScale = 1f;

            if (useDynamicWidth)
            {
                var minTargetAspect = targetWidth / (float)targetHeight;
                var maxTargetAspect = dynamicMaxWidth / (float)targetHeight;
                targetAspect = Mathf.Clamp(screenAspect, minTargetAspect, maxTargetAspect);
            }
            else
            {
                targetAspect = targetWidth / (float)targetHeight;
            }

            var scaleValue = screenAspect / targetAspect;

            Rect rect = new();
            if (scaleValue < 1f)
            {
                if (useDynamicHeight)
                {
                    var minTargetAspect = targetWidth / (float)dynamicMaxHeight;
                    if (screenAspect < minTargetAspect)
                    {
                        scaleValue = screenAspect / minTargetAspect;
                        orthoScale = minTargetAspect / targetAspect;
                    }
                    else
                    {
                        orthoScale = scaleValue;
                        scaleValue = 1f;
                    }
                }

                rect.width = 1;
                rect.height = scaleValue;
                rect.x = 0;
                rect.y = (1 - scaleValue) / 2;
            }
            else
            {
                scaleValue = 1 / scaleValue;
                rect.width = scaleValue;
                rect.height = 1;
                rect.x = (1 - scaleValue) / 2;
                rect.y = 0;
            }

            camera.orthographicSize = _orthoSize / orthoScale;
            camera.rect = rect;
        }
    }
}