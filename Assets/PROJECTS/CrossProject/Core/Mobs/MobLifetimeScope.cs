using UnityEngine;
using VContainer;
using VContainer.Unity;

namespace CrossProject.Core
{
    public class MobLifetimeScope : LifetimeScope
    {
        [SerializeField] private MobConfig _mobConfig;
        [SerializeField] private MobStateTimeConfig _mobStateTime;
        [SerializeField] private MobTransitionsConfig _mobTransitionsConfig;

        protected override void Configure(IContainerBuilder builder)
        {
            base.Configure(builder);

            BindStateMachine(builder);
            BindStates(builder);
            BindTransitions(builder);
            BindStateBindings(builder);
            BindAbilities(builder);
            BindConfigs(builder);
            BindHandlers(builder);
        }

        private void BindStateMachine(IContainerBuilder builder)
        {
            builder.Register<MobPersistentData>(Lifetime.Scoped).AsImplementedInterfaces();
            builder.Register<MobPerUpdateData>(Lifetime.Scoped).AsImplementedInterfaces();

            builder.Register<MobStateMachine>(Lifetime.Scoped).AsImplementedInterfaces();
            builder.Register<MobStateTimingHandler>(Lifetime.Scoped).AsImplementedInterfaces();
            builder.Register<MobTransitionsHolder>(Lifetime.Scoped).AsImplementedInterfaces();
        }

        private void BindStates(IContainerBuilder builder)
        {
            builder.Register<ApproachTargetMobState>(Lifetime.Scoped).AsImplementedInterfaces();
            builder.Register<AttackMobState>(Lifetime.Scoped).AsImplementedInterfaces();
            builder.Register<AttackPauseMobState>(Lifetime.Scoped).AsImplementedInterfaces();
            builder.Register<NoticeMobState>(Lifetime.Scoped).AsImplementedInterfaces();
            builder.Register<RotateToTargetMobState>(Lifetime.Scoped).AsImplementedInterfaces();
            builder.Register<WaitForTargetMobState>(Lifetime.Scoped).AsImplementedInterfaces();
            builder.Register<IdleMobState>(Lifetime.Scoped).AsImplementedInterfaces();
            builder.Register<ReturnToRoamAreaMobState>(Lifetime.Scoped).AsImplementedInterfaces();
            builder.Register<RoamMobState>(Lifetime.Scoped).AsImplementedInterfaces();
            builder.Register<RoamRotationMobState>(Lifetime.Scoped).AsImplementedInterfaces();
            builder.Register<DieMobState>(Lifetime.Scoped).AsImplementedInterfaces();
            builder.Register<DeadMobState>(Lifetime.Scoped).AsImplementedInterfaces();
            builder.Register<ResponsiveNoticeMobState>(Lifetime.Scoped).AsImplementedInterfaces();
        }

        private void BindTransitions(IContainerBuilder builder)
        {
            builder.Register<ApproachTargetMobTransition>(Lifetime.Scoped).AsImplementedInterfaces();
            builder.Register<AttackMobTransition>(Lifetime.Scoped).AsImplementedInterfaces();
            builder.Register<AttackPauseMobTransition>(Lifetime.Scoped).AsImplementedInterfaces();
            builder.Register<NoticeMobTransition>(Lifetime.Scoped).AsImplementedInterfaces();
            builder.Register<RotateToTargetMobTransition>(Lifetime.Scoped).AsImplementedInterfaces();
            builder.Register<WaitForTargetMobTransition>(Lifetime.Scoped).AsImplementedInterfaces();
            builder.Register<IdleMobTransition>(Lifetime.Scoped).AsImplementedInterfaces();
            builder.Register<ReturnToRoamAreaMobTransition>(Lifetime.Scoped).AsImplementedInterfaces();
            builder.Register<RoamMobTransition>(Lifetime.Scoped).AsImplementedInterfaces();
            builder.Register<RoamRotationMobTransition>(Lifetime.Scoped).AsImplementedInterfaces();
            builder.Register<DieMobTransition>(Lifetime.Scoped).AsImplementedInterfaces();
            builder.Register<DeadMobTransition>(Lifetime.Scoped).AsImplementedInterfaces();
            builder.Register<ResponsiveNoticeMobTransition>(Lifetime.Scoped).AsImplementedInterfaces();
        }

        private void BindStateBindings(IContainerBuilder builder)
        {
            builder.Register<MobAnimationHandler>(Lifetime.Scoped).AsImplementedInterfaces();
        }

        private void BindAbilities(IContainerBuilder builder)
        {
            builder.Register<AttackAbility>(Lifetime.Scoped).AsImplementedInterfaces();
            builder.Register<MoveAbility>(Lifetime.Scoped).AsImplementedInterfaces();
            builder.Register<NoticeEnemyArea>(Lifetime.Scoped).AsImplementedInterfaces();
            builder.Register<RoamSphereArea>(Lifetime.Scoped).AsImplementedInterfaces();
            builder.Register<RotateAbility>(Lifetime.Scoped).AsImplementedInterfaces();
            builder.Register<DieAbility>(Lifetime.Scoped).AsImplementedInterfaces();
            builder.Register<AgroArea>(Lifetime.Scoped).AsImplementedInterfaces();
        }

        private void BindConfigs(IContainerBuilder builder)
        {
            builder.RegisterInstance(_mobConfig);
            builder.RegisterInstance(_mobStateTime);
            builder.RegisterInstance(_mobTransitionsConfig);
        }

        private void BindHandlers(IContainerBuilder builder)
        {
            builder.Register<HealthHandler>(Lifetime.Scoped).AsImplementedInterfaces();
            builder.Register<MobDamageInfoProvider>(Lifetime.Scoped).AsImplementedInterfaces();
            builder.Register<DamageReceiveHandler>(Lifetime.Scoped).AsImplementedInterfaces();
            builder.Register<AudioService>(Lifetime.Scoped).AsImplementedInterfaces();
        }
    }
}
