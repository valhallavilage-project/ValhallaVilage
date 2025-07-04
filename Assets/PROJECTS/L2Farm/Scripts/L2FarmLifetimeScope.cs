using CrossProject.Core;
using CrossProject.Core.Camera;
using CrossProject.Core.Cheats;
using CrossProject.Core.Interactions;
using CrossProject.Core.SimpleMovement;
using CrossProject.Ui.Core;
using CrossProject.Ui.Implementations;
using CrossProject.Ui.Implementations.InteractButton;
using VContainer;
using VContainer.Unity;

namespace L2Farm.Scripts
{
    public class L2FarmLifetimeScope : LifetimeScope
    {
        private void RegisterCheats(IContainerBuilder builder)
        {
            builder.Register<CameraCheatOptions>(Lifetime.Singleton)
                .AsImplementedInterfaces();

            builder.Register<BlockableCheatPanelReaction>(Lifetime.Singleton)
                .AsImplementedInterfaces();
        }

        protected override void Configure(IContainerBuilder builder)
        {
            builder.RegisterEntryPoint<AddressablesManager>()
                .AsSelf();

            builder.Register<NoInternetScreenController>(Lifetime.Singleton)
                .AsSelf()
                .AsImplementedInterfaces();

            //TODO : VM : Remote Config
            //TODO : VM : GameMigrate - prepare here, on gamestate load/create - migrate

            //TODO : VM : GameState

            //TODO : VM : InApp
            //TODO : VM : Ads
            //TODO : VM : ContentService

            builder.Register<ScenesService>(Lifetime.Singleton)
                .AsSelf();

            builder.RegisterComponentInHierarchy<CameraService>()
                .AsSelf()
                .AsImplementedInterfaces();

            builder.RegisterComponentInHierarchy<UiService>()
                .AsSelf()
                .AsImplementedInterfaces();

            builder.Register<L2FarmGameLoader>(Lifetime.Singleton)
                .AsSelf()
                .AsImplementedInterfaces();

            builder.Register<JoystickController>(Lifetime.Singleton)
                .AsSelf()
                .AsImplementedInterfaces();

            builder.RegisterComponentInHierarchy<SimpleMovementController>()
                .AsSelf()
                .AsImplementedInterfaces();

            builder.RegisterComponentInHierarchy<Interactor>()
                .AsSelf()
                .AsImplementedInterfaces();

            // builder.Register<InteractButtonController>(Lifetime.Singleton)
            //     .AsSelf()
            //     .AsImplementedInterfaces();

            RegisterCheats(builder);
        }
    }
}