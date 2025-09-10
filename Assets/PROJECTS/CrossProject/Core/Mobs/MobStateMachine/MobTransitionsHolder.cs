using System;
using System.Collections.Generic;

namespace CrossProject.Core
{
    public interface IMobTransitionsHolder : ITransitionsHolder<MobState, MobTransition, IMobTransition>
    {
    }

    public class MobTransitionsHolder : BaseTransitionsHolder<MobState, MobTransition, IMobTransition>, IMobTransitionsHolder
    {
        public MobTransitionsHolder(IEnumerable<IMobTransition> transitions, MobTransitionsConfig transitionsConfig)
            : base(transitions, transitionsConfig)
        {
        }

        protected override bool CompareTransitions(Enum transition1, Enum transition2)
        {
            return (MobTransition)transition1 == (MobTransition)transition2;
        }
    }
}
