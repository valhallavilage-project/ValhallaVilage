using CrossProject.Core.Pooling;
using Cysharp.Threading.Tasks;

namespace CrossProject.Core
{
    public interface IDieAbility
    {
        IReadOnlyAsyncReactiveProperty<bool> Dead { get; }
        
        void DeadCompletely();
    }

    public class DieAbility : IDieAbility
    {
        private readonly AsyncReactiveProperty<bool> _dead = new(default);

        public IReadOnlyAsyncReactiveProperty<bool> Dead => _dead;

        public void DeadCompletely()
        {
            _dead.Value = true;
        }
    }
}
