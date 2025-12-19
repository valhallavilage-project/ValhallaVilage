using System;
using System.Collections.Generic;
using System.Linq;
using Unity.Collections.LowLevel.Unsafe;
using UnityEngine;

namespace CrossProject.Core
{
    public interface ITransitionsHolder<in TStateType, out TTransitionType, out TTransition>
        where TStateType : Enum
        where TTransitionType : Enum
        where TTransition : ITransition<TStateType, TTransitionType>
    {
        IEnumerable<TTransition> GetTransitions(TStateType state);
    }

    public abstract class BaseTransitionsHolder<TStateType, TTransitionType, TTransition> : ITransitionsHolder<TStateType, TTransitionType, TTransition>
        where TStateType : Enum
        where TTransitionType : Enum
        where TTransition : ITransition<TStateType, TTransitionType>
    {
        private readonly ITransitionsConfig<TStateType, TTransitionType> _transitionsConfig;
        private readonly IEnumerable<TTransition> _transitions;
        private readonly Dictionary<int, TTransition[]> _transitionsForState = new();

        protected BaseTransitionsHolder(IEnumerable<TTransition> transitions, ITransitionsConfig<TStateType, TTransitionType> transitionsConfig)
        {
            _transitions = transitions;
            _transitionsConfig = transitionsConfig;
        }

        public IEnumerable<TTransition> GetTransitions(TStateType state)
        {
            var stateInt = UnsafeUtility.As<TStateType, int>(ref state);

            if (!_transitionsForState.ContainsKey(stateInt))
            {
                if (!_transitionsConfig.ContainsState(state))
                {
                    Debug.Log($"No Transitions for state: {state}");

                    _transitionsForState[stateInt] = Array.Empty<TTransition>();
                }
                else
                {
                    _transitionsForState[stateInt] = _transitionsConfig.GetTransitions(state)
                        .Select(transition => _transitions.FirstOrDefault(a => CompareTransitions(a.Transition, transition)))
                        .Where(t => t != null)
                        .ToArray();
                }
            }

            return _transitionsForState[stateInt];
        }

        protected abstract bool CompareTransitions(Enum transition1, Enum transition2);
    }
}