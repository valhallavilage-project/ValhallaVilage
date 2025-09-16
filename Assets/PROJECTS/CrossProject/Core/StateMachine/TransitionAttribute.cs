using System;

namespace CrossProject.Core
{
    public class TransitionAttribute : Attribute
    {
        public int Transition { get; }

        public TransitionAttribute(int transition)
        {
            Transition = transition;
        }
    }
}