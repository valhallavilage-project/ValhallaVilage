using System;
using Cysharp.Threading.Tasks;

namespace CrossProject.Core
{
    public interface IStateMachine<TStateType>
        where TStateType : Enum
    {
        IReadOnlyAsyncReactiveProperty<TStateType> CurrentState { get; }
        TStateType PreviousState { get; }

        UniTask EnterState(TStateType state);
        UniTask Execute();
        UniTask ExitState(TStateType state);
        UniTask TryTransitionToState(TStateType state);
        void ToDefaultState();
    }
}
