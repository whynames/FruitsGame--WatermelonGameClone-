using _Assets.Scripts.Misc;
using _Assets.Scripts.Sprites;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace _Assets.Scripts.Services.Providers
{
    public class MainMenuProvider
    {
        private Sprite _backgroundSprite;
        private readonly SpriteServiceSettings _spriteServiceSettings;

        public Sprite BackgroundSprite => _backgroundSprite;

        public MainMenuProvider(SpriteServiceSettings spriteServiceSettings) => _spriteServiceSettings = spriteServiceSettings;

        public void Load() => _backgroundSprite = _spriteServiceSettings.BackgroundSprite;
    }
}