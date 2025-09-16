using System.Collections.Generic;

namespace CrossProject.Core
{
    public interface IMobStateMachine : IStateMachine<MobState>
    {
    }

    public class MobStateMachine : BaseStateMachine<IMobState, MobState>, IMobStateMachine
    {
        public MobStateMachine(IEnumerable<IMobState> states) : base(states)
        {
        }
    }
}
