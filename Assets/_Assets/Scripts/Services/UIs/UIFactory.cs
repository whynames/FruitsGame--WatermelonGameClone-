using _Assets.Scripts.Services.Configs;
using _Assets.Scripts.Services.Providers;
using _Assets.Scripts.Services.UIs.InGame;
using _Assets.Scripts.Services.UIs.Mods;
using Cysharp.Threading.Tasks;
using VContainer;
using VContainer.Unity;

namespace _Assets.Scripts.Services.UIs
{
    public class UIMenuFactory
    {
        private readonly ConfigProvider _configProvider;
        private readonly IObjectResolver _objectResolver;
        private readonly LoadingCurtainIconProvider _loadingCurtainIconProvider;
        private readonly MainMenuProvider _mainMenuProvider;

        private UIMenuFactory(ConfigProvider configProvider, IObjectResolver objectResolver, LoadingCurtainIconProvider loadingCurtainIconProvider, MainMenuProvider mainMenuProvider)
        {
            _configProvider = configProvider;
            _objectResolver = objectResolver;
            _loadingCurtainIconProvider = loadingCurtainIconProvider;
            _mainMenuProvider = mainMenuProvider;
        }

        public async UniTask<LoadingCurtain> CreateLoadingCurtain()
        {
            _loadingCurtainIconProvider.Load();
            var ui = _objectResolver.Instantiate(_configProvider.UIConfig.LoadingCurtain);
            ui.GetComponent<LoadingCurtain>().Init(_loadingCurtainIconProvider.BackgroundSprite, _loadingCurtainIconProvider.IconSprite);
            ui.GetComponent<LoadingCurtain>().Show();
            return ui.GetComponent<LoadingCurtain>();
        }

        public async UniTask<MainMenu> CreateMainMenuUI()
        {
            _mainMenuProvider.Load();
            var ui = _objectResolver.Instantiate(_configProvider.UIConfig.MainMenu);
            ui.GetComponent<MainMenu>().Init(_mainMenuProvider.BackgroundSprite);
            return ui.GetComponent<MainMenu>();
        }

        public SettingsMenu CreateSettingsMenu()
        {
            var ui = _objectResolver.Instantiate(_configProvider.UIConfig.SettingMenu);
            ui.GetComponent<SettingsMenu>().Init(_mainMenuProvider.BackgroundSprite);
            return ui.GetComponent<SettingsMenu>();
        }

        public async UniTask<InGameBackground> CreateInGameUI()
        {
            var ui = _objectResolver.Instantiate(_configProvider.UIConfig.InGameMenu);
            ui.GetComponent<NextFruitUI>().Init();
            ui.GetComponent<InGameTimer>().Init();
            ui.GetComponent<ScoreUI>().Init();
            ui.GetComponent<InGameBackground>().Init();
            return ui.GetComponent<InGameBackground>();
        }

        public InGamePauseMenu CreatePauseUI()
        {
            var ui = _objectResolver.Instantiate(_configProvider.UIConfig.PauseMenu);
            return ui.GetComponent<InGamePauseMenu>();
        }

        public GameOverMenu CreateGameOverUI()
        {
            return _objectResolver.Instantiate(_configProvider.UIConfig.GameOverMenu).GetComponent<GameOverMenu>();
        }

        public ModsMenu CreateModUI()
        {
            var modUI = _objectResolver.Instantiate(_configProvider.UIConfig.ModsMenu).GetComponent<ModsMenu>();
            modUI.Init(_mainMenuProvider.BackgroundSprite);
            return modUI;
        }
    }
}