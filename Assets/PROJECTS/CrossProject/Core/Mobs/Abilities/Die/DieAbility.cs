using CrossProject.Core.Pooling;
using Cysharp.Threading.Tasks;

namespace CrossProject.Core
{
    public interface IDieAbility
    {
        IReadOnlyAsyncReactiveProperty<bool> DeathBegan { get; }
        IReadOnlyAsyncReactiveProperty<bool> DeathCompleted { get; }

        void BeginToDie();
        void DeadCompletely();
    }

    public class DieAbility : IDieAbility
    {
        private readonly AsyncReactiveProperty<bool> _deathBegan = new(default);
        private readonly AsyncReactiveProperty<bool> _deathCompleted = new(default);

        public IReadOnlyAsyncReactiveProperty<bool> DeathBegan => _deathBegan;
        public IReadOnlyAsyncReactiveProperty<bool> DeathCompleted => _deathCompleted;

        public void BeginToDie()
        {
            _deathBegan.Value = true;
        }

        public void DeadCompletely()
        {
            _deathCompleted.Value = true;
        }
    }
}
