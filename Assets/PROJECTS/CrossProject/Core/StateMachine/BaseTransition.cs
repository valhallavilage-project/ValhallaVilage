using System;
using System.Collections.Generic;

namespace CrossProject.Core
{
    public interface ITransition<TStateType, out TTransitionType>
        where TStateType : Enum
        where TTransitionType : Enum
    {
        TTransitionType Transition { get; }
        TStateType State { get; }

        bool IsTransitionFromStateValid(TStateType inState);
    }

    public abstract class BaseTransition<TStateType, TTransitionType> : ITransition<TStateType, TTransitionType>
        where TStateType : Enum
        where TTransitionType : Enum
    {
        private IStateTimingHandler<TStateType> _stateTimeHandler;
        private IEnumerable<TStateType> _statesUsingTransition;
        private ITransitionsConfig<TStateType, TTransitionType> _config;

        public abstract TStateType State { get; }
        public abstract TTransitionType Transition { get; }

        protected virtual bool IsActive => true;
        protected Dictionary<TStateType, Func<bool>> ConditionForState { get; } = new();

        protected void Initialize(ITransitionsConfig<TStateType, TTransitionType> config, IStateTimingHandler<TStateType> stateDelayHandler)
        {
            _config = config;
            _statesUsingTransition = config.ContainsTransition(Transition) ? config.GetStatesForTransition(Transition) : new List<TStateType>(0);
            _stateTimeHandler = stateDelayHandler;

            FillConditionForStates();
        }

        public bool IsTransitionFromStateValid(TStateType inState)
        {
            var canBeInterrupted = _config.CanTransitionInterruptState(inState, Transition);

            return IsActive && (_stateTimeHandler.IsTimePassed(inState) || canBeInterrupted) && ConditionForState[inState]();
        }

        protected virtual bool Condition()
        {
            return true;
        }

        protected virtual void FillConditionForStates()
        {
            foreach (var state in _statesUsingTransition)
            {
                ConditionForState[state] = Condition;
            }
        }
    }
}