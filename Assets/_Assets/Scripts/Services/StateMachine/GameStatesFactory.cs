using _Assets.Scripts.Misc;
using _Assets.Scripts.Services.Audio;
using _Assets.Scripts.Services.Datas;
using _Assets.Scripts.Services.Datas.GameConfigs;
using _Assets.Scripts.Services.Datas.Mods;
using _Assets.Scripts.Services.Datas.Player;
using _Assets.Scripts.Services.Factories;
using _Assets.Scripts.Services.StateMachine.States;
using _Assets.Scripts.Services.UIs.StateMachine;
using _Assets.Scripts.Services.Vibrations;

namespace _Assets.Scripts.Services.StateMachine
{
    public class GameStatesFactory
    {
        private readonly IPlayerDataLoader _playerDataLoader;
        private readonly UIStateMachine _uiStateMachine;
        private readonly CoroutineRunner _coroutineRunner;
        private readonly PlayerFactory _playerFactory;
        private readonly ContainerFactory _containerFactory;
        private readonly ResetService _resetService;
        private readonly PlayerInput _playerInput;
        private readonly IConfigLoader _configLoader;
        private readonly AudioService _audioService;
        private readonly IModDataLoader _modDataLoader;
        private readonly IAudioSettingsLoader _audioSettingsLoader;
        private readonly ResumeGameService _resumeGameService;
        private readonly VibrationService _vibrationService;
        private readonly IVibrationSettingLoader _vibrationSettingLoader;

        private GameStatesFactory(IPlayerDataLoader playerDataLoader, UIStateMachine uiStateMachine,
            CoroutineRunner coroutineRunner, PlayerFactory playerFactory, ContainerFactory containerFactory,
            ResetService resetService, PlayerInput playerInput, IConfigLoader configLoader, AudioService audioService,
            IModDataLoader modDataLoader, IAudioSettingsLoader audioSettingsLoader, ResumeGameService resumeGameService, VibrationService vibrationService, IVibrationSettingLoader vibrationSettingLoader)
        {
            _playerDataLoader = playerDataLoader;
            _uiStateMachine = uiStateMachine;
            _coroutineRunner = coroutineRunner;
            _playerFactory = playerFactory;
            _containerFactory = containerFactory;
            _resetService = resetService;
            _playerInput = playerInput;
            _configLoader = configLoader;
            _audioService = audioService;
            _modDataLoader = modDataLoader;
            _audioSettingsLoader = audioSettingsLoader;
            _resumeGameService = resumeGameService;
            _vibrationService = vibrationService;
            _vibrationSettingLoader = vibrationSettingLoader;
        }

        public IGameState CreateLoadSaveDataState(GameStateMachine stateMachine)
        {
            return new LoadSaveDataState(stateMachine, _playerDataLoader, _uiStateMachine, _configLoader, _modDataLoader, _audioSettingsLoader, _resumeGameService, _vibrationSettingLoader);
        }

        public IGameState CreateInitState(GameStateMachine stateMachine)
        {
            return new InitState(stateMachine, _audioService, _audioSettingsLoader, _configLoader, _modDataLoader, _uiStateMachine, _vibrationService, _playerInput);
        }

        public IGameState CreateGameState(GameStateMachine stateMachine)
        {
            return new GameState(stateMachine, _uiStateMachine, _playerFactory, _containerFactory, _playerInput, _audioService);
        }

        public IGameState CreateGameOverState(GameStateMachine stateMachine)
        {
            return new GameOverState(stateMachine, _uiStateMachine, _playerInput, _resumeGameService);
        }

        public IGameState CreateGameOverAndMainMenuState(GameStateMachine stateMachine)
        {
            return new GameOverAndMainMenu(_resetService, _uiStateMachine, _audioService);
        }

        public IGameState CreateSaveDataState(GameStateMachine stateMachine)
        {
            return new SaveDataState(_playerDataLoader, _modDataLoader, _audioSettingsLoader, _resumeGameService, _vibrationSettingLoader);
        }

        public IGameState CreateResetAndRetryState(GameStateMachine stateMachine)
        {
            return new ResetAndRetry(stateMachine, _resetService);
        }

        public IGameState CreateResetAndMainMenuState(GameStateMachine stateMachine)
        {
            return new ResetAndMainMenu(stateMachine, _resetService, _uiStateMachine, _audioService, _resumeGameService);
        }

        public IGameState CreateGamePauseState(GameStateMachine stateMachine)
        {
            return new GamePauseState(_playerInput, _uiStateMachine);
        }

        public IGameState CreateGameResumeState(GameStateMachine stateMachine)
        {
            return new GameResumeState(_playerInput, _uiStateMachine, _audioSettingsLoader);
        }

        public IGameState CreateContinueGameState(GameStateMachine stateMachine)
        {
            return new ResumeGameState(_uiStateMachine, _containerFactory, _playerFactory, _playerInput, _resumeGameService);
        }
    }
}