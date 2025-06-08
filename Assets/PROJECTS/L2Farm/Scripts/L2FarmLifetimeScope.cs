using CrossProject.Core;
using CrossProject.Ui.Core;
using CrossProject.Ui.Implementations;
using UnityEngine;
using VContainer;
using VContainer.Unity;

namespace RUNNER.PROJECTS.L2Farm.Scripts
{
    /// <summary>
    /// This is an EntryPoint for L2Farm project
    /// This code should init all DoNotDestroyOnLoad or OneTimeCall scripts
    /// After all initializations - next scene should be loaded
    /// </summary>
    public class L2FarmLifetimeScope : LifetimeScope
    {
        [SerializeField] private UiService uiServicePrefab;

        protected override void Configure(IContainerBuilder builder)
        {
            builder.Register<NoInternetScreenController>(Lifetime.Singleton)
                .AsSelf();

            //TODO : VM : GameState
            //TODO : VM : Remote Config
            //TODO : VM : GameMigrate
            //TODO : VM : InApp
            //TODO : VM : Ads
            //TODO : VM : ContentService

            builder.RegisterEntryPoint<AddressablesManager>()
                .AsSelf();

            builder.Register<ScenesService>(Lifetime.Singleton)
                .AsSelf();

            builder.RegisterComponentInNewPrefab(uiServicePrefab, Lifetime.Singleton)
                .AsSelf()
                .AsImplementedInterfaces();

            builder.Register<LoadingScreenController>(Lifetime.Singleton)
                .AsSelf()
                .AsImplementedInterfaces();
        }
    }
}