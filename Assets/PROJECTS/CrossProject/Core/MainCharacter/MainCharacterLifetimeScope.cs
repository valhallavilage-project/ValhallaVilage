using UnityEngine;
using VContainer;
using VContainer.Unity;

namespace CrossProject.Core
{
    public class MainCharacterLifetimeScope : LifetimeScope
    {
        [SerializeField] private MainCharacterArmorSetsConfig _mainCharacterSets;
        [SerializeField] private EnergyRestorationConfig _energyRestorationConfig;
        [SerializeField] private HealthRestorationConfig _healthRestorationConfig;
        [SerializeField] private LevelProgressionConfig _levelProgressionConfig;
        [SerializeField] private PotionsConfig _potionsConfig;

        protected override void Configure(IContainerBuilder builder)
        {
            base.Configure(builder);

            BindAbilities(builder);
            BindServices(builder);
            BindConfigs(builder);
            
            #if !DISABLE_SRDEBUGGER
            BindCheats(builder);
            #endif
        }

        private void BindAbilities(IContainerBuilder builder)
        {
            builder.Register<ReviveAbility>(Lifetime.Scoped).AsImplementedInterfaces();
            builder.Register<DieAbility>(Lifetime.Scoped).AsImplementedInterfaces();
            builder.Register<AttackAbility>(Lifetime.Scoped).AsImplementedInterfaces();
        }

        private void BindServices(IContainerBuilder builder)
        {
            builder.Register<HealthHandler>(Lifetime.Scoped).AsImplementedInterfaces();
            builder.Register<RestoreHealthHandler>(Lifetime.Scoped).AsImplementedInterfaces();
            builder.Register<EnergyHandler>(Lifetime.Scoped).AsImplementedInterfaces();
            builder.Register<RestoreEnergyHandler>(Lifetime.Scoped).AsImplementedInterfaces();
            builder.Register<ExperienceHandler>(Lifetime.Scoped).AsImplementedInterfaces();

            builder.Register<MainCharacterAttackInteractionHandler>(Lifetime.Scoped).AsImplementedInterfaces();
            builder.Register<MainCharacterDamageInfoProvider>(Lifetime.Scoped).AsImplementedInterfaces();
            builder.Register<MainCharacterArmorSetsService>(Lifetime.Scoped).AsImplementedInterfaces();
            builder.Register<MainCharacterSharedDataHandler>(Lifetime.Scoped).AsImplementedInterfaces();
            builder.Register<MainCharacterSaveEnergyHandler>(Lifetime.Scoped).AsImplementedInterfaces();
            builder.Register<MainCharacterSaveHealthHandler>(Lifetime.Scoped).AsImplementedInterfaces();
            builder.Register<MainCharacterSaveExperienceHandler>(Lifetime.Scoped).AsImplementedInterfaces();

            builder.Register<MainCharacterGlobalParameterChangesHandler>(Lifetime.Scoped).AsImplementedInterfaces();
            builder.Register<DamageReceiveHandler>(Lifetime.Scoped).AsImplementedInterfaces();
            builder.Register<AudioService>(Lifetime.Scoped).AsImplementedInterfaces();
        }

        private void BindConfigs(IContainerBuilder builder)
        {
            builder.RegisterInstance(_mainCharacterSets);
            builder.RegisterInstance(_energyRestorationConfig);
            builder.RegisterInstance(_healthRestorationConfig);
            builder.RegisterInstance(_levelProgressionConfig);
            builder.RegisterInstance(_potionsConfig);
        }

        #if !DISABLE_SRDEBUGGER
        private void BindCheats(IContainerBuilder builder)
        {
            builder.Register<MainCharacterCheatOptions>(Lifetime.Singleton)
                .AsImplementedInterfaces();
        }
        #endif
    }
}
