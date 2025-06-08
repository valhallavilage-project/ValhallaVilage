using CrossProject.Core;
using CrossProject.Ui.Core;
using CrossProject.Ui.Implementations;
using UnityEngine;
using VContainer;
using VContainer.Unity;

namespace RUNNER.PROJECTS.L2Farm.Scripts
{
    public class L2FarmLifetimeScope : LifetimeScope
    {
        [SerializeField] private UiService uiServicePrefab;

        protected override void Configure(IContainerBuilder builder)
        {
            builder.RegisterEntryPoint<AddressablesManager>()
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