using Cysharp.Threading.Tasks;

namespace CrossProject.Core
{
    public interface IDieAbility
    {
        IReadOnlyAsyncReactiveProperty<Invoker> DeathBegan { get; }
        IReadOnlyAsyncReactiveProperty<Invoker> DeathCompleted { get; }

        void BeginToDie();
        void DeadCompletely();
    }

    public class DieAbility : IDieAbility
    {
        private readonly AsyncReactiveProperty<Invoker> _deathBegan = new(default);
        private readonly AsyncReactiveProperty<Invoker> _deathCompleted = new(default);

        public IReadOnlyAsyncReactiveProperty<Invoker> DeathBegan => _deathBegan;
        public IReadOnlyAsyncReactiveProperty<Invoker> DeathCompleted => _deathCompleted;

        public void BeginToDie()
        {
            _deathBegan.Invoke();
        }

        public void DeadCompletely()
        {
            _deathCompleted.Invoke();
        }
    }
}
