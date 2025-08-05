using CrossProject.Core;
using CrossProject.Core.Audio;
using CrossProject.Core.Camera;
using CrossProject.Core.Characters;
using CrossProject.Core.Cheats;
using CrossProject.Core.Interactions;
using CrossProject.Core.PROJECTS.CrossProject.Core;
using CrossProject.Core.SaveLoad;
using CrossProject.Core.SimpleMovement;
using CrossProject.Core.Skins;
using CrossProject.Ui.Core;
using CrossProject.Ui.Implementations;
using CrossProject.Ui.Implementations.InteractButton;
using CrossProject.Ui.Implementations.SettingsPopup;
using Cysharp.Threading.Tasks;
using L2Farm.Features.InventoryScreen;
using L2Farm.Features.QuestsScreen;
using L2Farm.Features.ShopScreen;
using L2Farm.Scripts.CharacterHudElement;
using PROJECTS.L2Farm.Scripts.CharacterSkinSelect;
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

            builder.Register<JoystickCheatOptions>(Lifetime.Singleton)
                .AsImplementedInterfaces();

            builder.Register<SimpleMovementCheatOptions>(Lifetime.Singleton)
                .AsImplementedInterfaces();

            builder.Register<BlockableCheatPanelReaction>(Lifetime.Singleton)
                .AsImplementedInterfaces();
        }

        protected override void Configure(IContainerBuilder builder)
        {
            builder.RegisterEntryPoint<EmptyEntryPoint>()
                .AsSelf();

            builder.RegisterComponentInHierarchy<ManualPrefabInjector>()
                .AsSelf();

            builder.Register<AddressablesManager>(Lifetime.Singleton)
                .AsSelf()
                .AsImplementedInterfaces();

            builder.Register<CharactersService>(Lifetime.Singleton)
                .AsSelf()
                .AsImplementedInterfaces();

            builder.Register<SkinService>(Lifetime.Singleton)
                .AsSelf()
                .AsImplementedInterfaces();

            builder.Register<NoInternetScreenController>(Lifetime.Singleton)
                .AsSelf()
                .AsImplementedInterfaces();

            //TODO : VM : Remote Config
            //TODO : VM : GameMigrate - prepare here, on gamestate load/create - migrate

            builder.Register<GameStateManager>(Lifetime.Singleton)
                .AsSelf()
                .AsImplementedInterfaces();

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

            builder.RegisterComponentInHierarchy<AudioManager>()
                .AsSelf()
                .AsImplementedInterfaces();

            builder.Register<L2FarmGameLoader>(Lifetime.Singleton)
                .AsSelf()
                .AsImplementedInterfaces();

            builder.Register<SettingsPopupController>(Lifetime.Singleton)
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

            builder.Register<InteractButtonController>(Lifetime.Singleton)
                .AsSelf()
                .AsImplementedInterfaces();

            builder.Register<CharacterSkinSelectScreenController>(Lifetime.Singleton)
                .AsSelf()
                .AsImplementedInterfaces();

            builder.Register<CharacterHudElementController>(Lifetime.Singleton)
                .AsSelf()
                .AsImplementedInterfaces();

            builder.Register<InventoryScreenController>(Lifetime.Singleton)
                .AsSelf()
                .AsImplementedInterfaces();

            builder.Register<QuestsPopupController>(Lifetime.Singleton)
                .AsSelf()
                .AsImplementedInterfaces();

            builder.Register<ShopScreenController>(Lifetime.Singleton)
                .AsSelf()
                .AsImplementedInterfaces();

            RegisterCheats(builder);
        }
    }
}