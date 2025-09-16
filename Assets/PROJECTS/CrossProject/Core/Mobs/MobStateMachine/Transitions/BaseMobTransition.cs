using VContainer;

namespace CrossProject.Core
{
    public interface IMobTransition : ITransition<MobState, MobTransition>
    {
    }
    
    public abstract class BaseMobTransition : BaseTransition<MobState, MobTransition>, IMobTransition
    {
        [Inject]
        public void AddDependencies(MobTransitionsConfig config, IMobStateTimingHandler stateDelayHandler)
        {
            Initialize(config, stateDelayHandler);
        }
    }
}