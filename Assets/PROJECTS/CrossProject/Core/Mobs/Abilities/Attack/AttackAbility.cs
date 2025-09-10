using Cysharp.Threading.Tasks;

namespace CrossProject.Core
{
    public interface IAttackAbility
    {
        IReadOnlyAsyncReactiveProperty<bool> AttackBegin { get; }
        IReadOnlyAsyncReactiveProperty<bool> AttackEnd { get; }
        
        void BeginAttack();
        void EndAttack();
    }

    public class AttackAbility : IAttackAbility
    {
        private readonly AsyncReactiveProperty<bool> _attackBegin = new(default);
        private readonly AsyncReactiveProperty<bool> _attackEnd = new(default);

        public IReadOnlyAsyncReactiveProperty<bool> AttackBegin => _attackBegin;
        public IReadOnlyAsyncReactiveProperty<bool> AttackEnd => _attackEnd;
        
        public void BeginAttack()
        {
            _attackBegin.Value = true;
        }

        public void EndAttack()
        {
            _attackEnd.Value = true;
        }
    }
}