using VContainer;
using VContainer.Unity;

namespace CrossProject.Core
{
    public class MainCharacterLifetimeScope : LifetimeScope
    {
        protected override void Configure(IContainerBuilder builder)
        {
            base.Configure(builder);

            BindAbilities(builder);
            BindHandlers(builder);
        }

        private void BindAbilities(IContainerBuilder builder)
        {
            builder.Register<AttackAbility>(Lifetime.Scoped).AsImplementedInterfaces();
        }

        private void BindHandlers(IContainerBuilder builder)
        {
            builder.Register<HealthHandler>(Lifetime.Scoped).AsImplementedInterfaces();
            builder.Register<MainCharacterAttackInteractionHandler>(Lifetime.Scoped).AsImplementedInterfaces().Build();
            builder.Register<MainCharacterDamageInfoProvider>(Lifetime.Scoped).AsImplementedInterfaces();
        }
    }
}
