using Cysharp.Threading.Tasks;

namespace CrossProject.Core
{
    public interface IAttackAbility
    {
        IReadOnlyAsyncReactiveProperty<Invoker> AttackBegin { get; }
        IReadOnlyAsyncReactiveProperty<Invoker> AttackEnd { get; }
        
        void BeginAttack();
        void EndAttack();
    }

    public class AttackAbility : IAttackAbility
    {
        private readonly AsyncReactiveProperty<Invoker> _attackBegin = new(default);
        private readonly AsyncReactiveProperty<Invoker> _attackEnd = new(default);

        public IReadOnlyAsyncReactiveProperty<Invoker> AttackBegin => _attackBegin;
        public IReadOnlyAsyncReactiveProperty<Invoker> AttackEnd => _attackEnd;
        
        public void BeginAttack()
        {
            _attackBegin.Invoke();
        }

        public void EndAttack()
        {
            _attackEnd.Invoke();
        }
    }
}