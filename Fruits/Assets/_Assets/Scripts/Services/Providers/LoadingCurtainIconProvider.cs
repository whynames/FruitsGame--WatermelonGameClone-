using _Assets.Scripts.Misc;
using _Assets.Scripts.Sprites;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace _Assets.Scripts.Services.Providers
{
    public class LoadingCurtainIconProvider
    {
        private Sprite _backgroundSprite;
        private Sprite _iconSprite;
        private readonly SpriteServiceSettings _spriteServiceSettings;


        public Sprite BackgroundSprite => _backgroundSprite;
        public Sprite IconSprite => _iconSprite;
        private LoadingCurtainIconProvider(SpriteServiceSettings spriteServiceSettings)
        {

            _spriteServiceSettings = spriteServiceSettings;
        }

        public async UniTask Load()
        {
            _backgroundSprite = _spriteServiceSettings.BackgroundSprite;
            _iconSprite = _spriteServiceSettings.PlayerSprite;
        }
    }
}