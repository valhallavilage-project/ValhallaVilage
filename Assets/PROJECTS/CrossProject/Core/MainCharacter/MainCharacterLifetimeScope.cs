using UnityEngine;
using VContainer;
using VContainer.Unity;

namespace CrossProject.Core
{
    public class MainCharacterLifetimeScope : LifetimeScope
    {
        [SerializeField] private MainCharacterClothesSetConfigFacade _mainCharacterSets;

        protected override void Configure(IContainerBuilder builder)
        {
            base.Configure(builder);

            BindAbilities(builder);
            BindServices(builder);
            BindConfigs(builder);
        }

        private void BindAbilities(IContainerBuilder builder)
        {
            builder.Register<AttackAbility>(Lifetime.Scoped).AsImplementedInterfaces();
        }

        private void BindServices(IContainerBuilder builder)
        {
            builder.Register<HealthHandler>(Lifetime.Scoped).AsImplementedInterfaces();
            builder.Register<EnergyHandler>(Lifetime.Scoped).AsImplementedInterfaces();
            builder.Register<MainCharacterAttackInteractionHandler>(Lifetime.Scoped).AsImplementedInterfaces().Build();
            builder.Register<MainCharacterDamageInfoProvider>(Lifetime.Scoped).AsImplementedInterfaces();
            builder.Register<MainCharacterClothesSetsService>(Lifetime.Scoped).AsImplementedInterfaces();
            builder.Register<MainCharacterSharedDataHandler>(Lifetime.Scoped).AsImplementedInterfaces();
        }

        private void BindConfigs(IContainerBuilder builder)
        {
            builder.RegisterInstance(_mainCharacterSets);
        }
    }
}
