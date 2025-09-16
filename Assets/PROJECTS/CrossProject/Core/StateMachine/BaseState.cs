using System;
using Cysharp.Threading.Tasks;

namespace CrossProject.Core
{
    public abstract class BaseState<TStateType> : IState<TStateType>
        where TStateType : Enum
    {
        public abstract TStateType State { get; }

        public virtual UniTask Enter()
        {
            return UniTask.CompletedTask;
        }

        public async UniTask<TStateType> Execute()
        {
            await HandleControl();

            return await TryMakeTransition();
        }

        protected virtual UniTask HandleControl()
        {
            return UniTask.CompletedTask;
        }

        protected virtual UniTask<TStateType> TryMakeTransition()
        {
            return new UniTask<TStateType>(State);
        }

        public virtual UniTask Exit()
        {
            return UniTask.CompletedTask;
        }
    }
}
