using System;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;

namespace CrossProject.Core
{
    public abstract class BaseStateMachine<TState, TStateType> : IStateMachine<TStateType>
        where TState : IState<TStateType>
        where TStateType : Enum
    {
        private readonly AsyncReactiveProperty<TStateType> _currentState = new AsyncReactiveProperty<TStateType>(default);

        private TStateType _scheduledState;
        private bool _isEnterStateScheduled;
        private readonly Dictionary<TStateType, TState> _states = new Dictionary<TStateType, TState>();

        public IReadOnlyAsyncReactiveProperty<TStateType> CurrentState => _currentState;
        public TStateType PreviousState { get; private set; }

        protected BaseStateMachine(IEnumerable<TState> states)
        {
            foreach (var state in states)
            {
                _states.Add(state.State, state);
            }

            _isEnterStateScheduled = true;
        }

        public async UniTask EnterState(TStateType stateType)
        {
            await _states[stateType].Enter();
        }

        public async UniTask Execute()
        {
            TStateType nextState;

            if (_isEnterStateScheduled)
            {
                PreviousState = _currentState.Value;

                await EnterState(_scheduledState);
                
                nextState = await _states[_scheduledState].Execute();
                
                _currentState.Value = _scheduledState;

                _isEnterStateScheduled = false;
            }
            else
            {
                nextState = await _states[_currentState.Value].Execute();
            }

            await TryTransitionToState(nextState);
        }

        public async UniTask ExitState(TStateType state)
        {
            await _states[state].Exit();
        }

        public async UniTask TryTransitionToState(TStateType state)
        {
            if (Equals(CurrentState.Value, state))
            {
                return;
            }

            await ExitState(CurrentState.Value);

            _scheduledState = state;
            _isEnterStateScheduled = true;
        }

        public void ToDefaultState()
        {
            _scheduledState = default;
            _isEnterStateScheduled = true;
        }
    }
}
