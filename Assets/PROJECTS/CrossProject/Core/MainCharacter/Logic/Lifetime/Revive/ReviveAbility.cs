using Cysharp.Threading.Tasks;
using UnityEngine;

namespace CrossProject.Core
{
    public interface IReviveAbility
    {
        IReadOnlyAsyncReactiveProperty<Vector3> Revived { get; }
        
        void Revive(Vector3 position);
    }

    public class ReviveAbility : IReviveAbility
    {
        private readonly IHealthHandler _healthHandler;
        private readonly AsyncReactiveProperty<Vector3> _revived = new(default);

        public IReadOnlyAsyncReactiveProperty<Vector3> Revived => _revived;
        
        public ReviveAbility(IHealthHandler healthHandler)
        {
            _healthHandler = healthHandler;
        }

        public void Revive(Vector3 position)
        {
            _healthHandler.Restore(_healthHandler.MaxHealth.Value / 2);

            _revived.Value = position;
        }
    }
}
