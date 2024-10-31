using _Assets.Scripts.Services.Datas.GameConfigs;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace _Assets.Scripts.Misc
{
    public class SpriteCreator
    {
        private readonly IConfigLoader _configLoader;

        public SpriteCreator(IConfigLoader configLoader) => _configLoader = configLoader;

        public async UniTask<Sprite> CreateContainerSprite()
        {
            if (!_configLoader.IsDefault)
            {
                var path = _configLoader.CurrentConfig.ContainerImagePath;
                return await SpriteHelper.CreateSprite(path, true, true);
            }
            else
            {
                var path = _configLoader.CurrentConfig.ContainerImagePath;
                return await SpriteHelper.CreateSpriteFromStreamingAssests(path, true, true);
            }
        }

        public async UniTask<Sprite> CreateInGameBackground()
        {
            if (!_configLoader.IsDefault)
            {
                var path = _configLoader.CurrentConfig.InGameBackgroundPath;
                return await SpriteHelper.CreateSprite(path, false);
            }
            else
            {
                var path = _configLoader.CurrentConfig.InGameBackgroundPath;
                return await SpriteHelper.CreateSpriteFromStreamingAssests(path, false);
            }
        }

        public async UniTask<Sprite> CreateLoadingCurtainIcon()
        {
            if (!_configLoader.IsDefault)
            {
                var path = _configLoader.CurrentConfig.LoadingScreenIconPath;
                return await SpriteHelper.CreateSprite(path);
            }
            else
            {
                var path = _configLoader.CurrentConfig.LoadingScreenIconPath;
                return await SpriteHelper.CreateSpriteFromStreamingAssests(path);
            }
        }

        public async UniTask<Sprite> CreateLoadingCurtainBackground()
        {
            if (!_configLoader.IsDefault)
            {
                var path = _configLoader.CurrentConfig.LoadingScreenBackgroundPath;
                return await SpriteHelper.CreateSprite(path, false);
            }
            else
            {
                var path = _configLoader.CurrentConfig.LoadingScreenBackgroundPath;
                return await SpriteHelper.CreateSpriteFromStreamingAssests(path, false);
            }
        }

        public async UniTask<Sprite> CreatePlayerSkin()
        {
            if (!_configLoader.IsDefault)
            {
                var path = _configLoader.CurrentConfig.PlayerSkinPath;
                return await SpriteHelper.CreateSprite(path);
            }
            else
            {
                var path = _configLoader.CurrentConfig.PlayerSkinPath;
                return await SpriteHelper.CreateSpriteFromStreamingAssests(path);
            }
        }

        public async UniTask<Sprite> CreateMainMenuBackground()
        {
            if (!_configLoader.IsDefault)
            {
                var path = _configLoader.CurrentConfig.MainMenuBackgroundPath;
                return await SpriteHelper.CreateSprite(path, false);
            }
            else
            {
                var path = _configLoader.CurrentConfig.MainMenuBackgroundPath;
                return await SpriteHelper.CreateSpriteFromStreamingAssests(path, false);
            }
        }
    }
}