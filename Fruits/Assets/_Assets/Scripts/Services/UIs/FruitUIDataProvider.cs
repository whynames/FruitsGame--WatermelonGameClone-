using _Assets.Scripts.Misc;
using _Assets.Scripts.Services.Configs;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace _Assets.Scripts.Services.UIs
{
    public class FruitUIDataProvider
    {
        private readonly RandomNumberGenerator _randomNumberGenerator;
        private readonly SpritesCacheService _spritesCacheService;
        private readonly ConfigProvider _configProvider;



        private FruitUIDataProvider(RandomNumberGenerator randomNumberGenerator, SpritesCacheService spritesCacheService, ConfigProvider configProvider)
        {
            _randomNumberGenerator = randomNumberGenerator;
            _spritesCacheService = spritesCacheService;
            _configProvider = configProvider;
        }

        public Sprite GetCurrentFruit()
        {
            var current = _randomNumberGenerator.Current;
            var fruit = _configProvider.FruitsConfig.GetPrefab(current);
            return fruit.SpriteRenderer.sprite;
        }

        public Sprite GetNextFruit()
        {
            var next = _randomNumberGenerator.Next;
            var fruit = _configProvider.FruitsConfig.GetPrefab(next);
            return fruit.SpriteRenderer.sprite;
        }
    }
}