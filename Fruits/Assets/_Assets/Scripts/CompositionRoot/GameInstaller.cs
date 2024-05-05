using _Assets.Scripts.Misc;
using _Assets.Scripts.Services;
using _Assets.Scripts.Services.Audio;
using _Assets.Scripts.Services.Effects;
using _Assets.Scripts.Services.Factories;
using _Assets.Scripts.Services.StateMachine;
using _Assets.Scripts.Services.UIs;
using _Assets.Scripts.Services.UIs.StateMachine;
using _Assets.Scripts.Services.Vibrations;
using _Assets.Scripts.Sprites;
using UnityEngine;
using VContainer;
using VContainer.Unity;

namespace _Assets.Scripts.CompositionRoot
{
    public class GameInstaller : LifetimeScope
    {
        [SerializeField] private CoroutineRunner coroutineRunner;
        [SerializeField] private PlayerFactory playerFactory;
        [SerializeField] private ContainerFactory containerFactory;
        [SerializeField] private ModSlotFactory modSlotFactory;
        [SerializeField] private AudioService audioService;
        [SerializeField] private FxService fxService;
        [SerializeField] private SpriteService spriteService;

        [SerializeField] private PlayerInput playerInput;
        [SerializeField] private GamepadRumbleService gamepadRumbleService;

        protected override void Configure(IContainerBuilder builder)
        {
            builder.RegisterComponent(coroutineRunner);
            builder.RegisterComponent(playerFactory);
            builder.RegisterComponent(containerFactory);
            builder.RegisterComponent(modSlotFactory);
            builder.RegisterComponent(audioService);
            builder.RegisterComponent(fxService);
            builder.RegisterComponent(spriteService);
            builder.RegisterComponent(playerInput);
            builder.RegisterComponent(gamepadRumbleService);
            builder.Register<VibrationSettingsDataLoaderJson>(Lifetime.Singleton).As<IVibrationSettingLoader>();
            builder.Register<VibrationService>(Lifetime.Singleton);

            builder.Register<FruitUIDataProvider>(Lifetime.Singleton);
            builder.Register<FruitsFactory>(Lifetime.Singleton);


            builder.Register<ResumeGameService>(Lifetime.Singleton);


            builder.RegisterEntryPoint<GameOverTimer>().AsSelf();
            builder.Register<ResetService>(Lifetime.Singleton);

            builder.Register<LeaderBoardService>(Lifetime.Singleton);

            builder.Register<UIMenuFactory>(Lifetime.Singleton);
            builder.Register<UIStatesFactory>(Lifetime.Singleton);
            builder.Register<UIStateMachine>(Lifetime.Singleton);

            builder.Register<GameStatesFactory>(Lifetime.Singleton);
            builder.Register<GameStateMachine>(Lifetime.Singleton);

            builder.RegisterEntryPoint<GameOverService>();
        }
    }
}