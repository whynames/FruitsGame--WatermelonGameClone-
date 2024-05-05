using _Assets.Scripts.Services.Datas.GameConfigs;
using _Assets.Scripts.Sprites;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace _Assets.Scripts.Misc
{
    public class SpritesCacheService
    {
        private readonly SpriteServiceSettings _spriteServiceSettings;
        private Sprite[] _fruitSprites;
        private Sprite[] _fruitIconsSprites;


        public SpriteServiceSettings SpriteServiceSettings { get => _spriteServiceSettings; }

        public SpritesCacheService(SpriteServiceSettings spriteServiceSettings)
        {
            _spriteServiceSettings = spriteServiceSettings;
        }

        public async UniTask Preload(GameConfig config, bool isDefault)
        {
            if (!isDefault)
            {
                for (int i = 0; i < config.FruitSkinsImagesPaths.Length; i++)
                {
                    var path = config.FruitSkinsImagesPaths[i];
                    _fruitSprites[i] = await SpriteHelper.CreateSprite(path);
                }

                for (int i = 0; i < config.FruitIconsPaths.Length; i++)
                {
                    var path = config.FruitIconsPaths[i];
                    _fruitIconsSprites[i] = await SpriteHelper.CreateSprite(path);
                }
            }
            else
            {
                for (int i = 0; i < config.FruitSkinsImagesPaths.Length; i++)
                {
                    var path = config.FruitSkinsImagesPaths[i];
                    _fruitSprites[i] = await SpriteHelper.CreateSpriteFromStreamingAssests(path);
                }


                for (int i = 0; i < config.FruitIconsPaths.Length; i++)
                {
                    var path = config.FruitIconsPaths[i];
                    _fruitIconsSprites[i] = await SpriteHelper.CreateSpriteFromStreamingAssests(path);
                }
            }
        }

        public Sprite GetFruitSprite(int index) => _fruitSprites[index];

        public Sprite GetFruitIconSprite(int index) => _fruitIconsSprites[index];

        public void Reset()
        {
            _fruitSprites = new Sprite[12];
            _fruitIconsSprites = new Sprite[12];
        }
    }
}