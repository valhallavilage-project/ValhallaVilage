using CrossProject.Core;
using CrossProject.Ui.Core;
using CrossProject.Ui.Implementations;
using UnityEngine;
using VContainer;
using VContainer.Unity;

public class BrainRotRunnerLifetimeScope : LifetimeScope
{
    [SerializeField] private UiService uiServicePrefab;

    protected override void Configure(IContainerBuilder builder)
    {
        builder.RegisterEntryPoint<AddressablesManager>()
            .AsSelf();

        builder.RegisterComponentInNewPrefab(uiServicePrefab, Lifetime.Singleton)
            .AsSelf()
            .AsImplementedInterfaces();

    }
}