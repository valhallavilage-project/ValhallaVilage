using CrossProject.Core;
using CrossProject.Core.SaveLoad;
using CrossProject.Ui.Core;
using CrossProject.Ui.Implementations;
using UnityEngine;
using VContainer;
using VContainer.Unity;

namespace L2Farm.Scripts
{
    public class L2FarmLifetimeScope : LifetimeScope
    {
        [SerializeField] private UiService uiServicePrefab;

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
            builder.Register<GameStateManager>(Lifetime.Singleton)
                .AsSelf();

            //TODO : VM : InApp
            //TODO : VM : Ads
            //TODO : VM : ContentService

            builder.Register<ScenesService>(Lifetime.Singleton)
                .AsSelf();

            builder.RegisterComponentInNewPrefab(uiServicePrefab, Lifetime.Singleton)
                .AsSelf()
                .AsImplementedInterfaces();

            builder.Register<LoadingScreenController>(Lifetime.Singleton)
                .AsSelf()
                .AsImplementedInterfaces();

            builder.Register<L2FarmGameLoader>(Lifetime.Singleton)
                .AsSelf()
                .AsImplementedInterfaces();
        }
    }
}