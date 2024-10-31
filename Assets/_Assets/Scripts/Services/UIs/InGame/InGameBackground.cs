using _Assets.Scripts.Misc;
using _Assets.Scripts.Sprites;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;
using VContainer;

namespace _Assets.Scripts.Services.UIs.InGame
{
    public class InGameBackground : MonoBehaviour
    {
        [SerializeField] private Image inGameBackground;
        [Inject] private SpriteServiceSettings _spriteServiceSettings;

        public void Init()
        {
            inGameBackground.sprite = _spriteServiceSettings.BackgroundSprite;
            Color newColor = inGameBackground.color;
            newColor.a = 255;
            inGameBackground.color = newColor;
        }
    }
}