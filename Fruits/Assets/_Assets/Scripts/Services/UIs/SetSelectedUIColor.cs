using _Assets.Scripts.Misc;
using UnityEngine;
using UnityEngine.UI;

namespace _Assets.Scripts.Services.UIs
{
    public class SetSelectedUIColor : MonoBehaviour
    {
        [SerializeField] private Component element;

        private void Awake()
        {
            if (element.TryGetComponent(out Button button))
            {
                var componentColors = button.colors;
                componentColors.selectedColor = ColorHelper.SelectedUIElementColor;
                button.colors = componentColors;
            }
            else if (element.TryGetComponent(out Slider slider))
            {
                var componentColors = slider.colors;
                componentColors.selectedColor = ColorHelper.SelectedUIElementColor;
                slider.colors = componentColors;
            }
        }
    }
}