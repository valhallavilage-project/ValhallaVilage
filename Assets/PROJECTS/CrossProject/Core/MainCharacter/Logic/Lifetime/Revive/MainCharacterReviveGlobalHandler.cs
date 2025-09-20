using Cysharp.Threading.Tasks;
using UnityEngine;

namespace CrossProject.Core
{
    public interface IMainCharacterReviveGlobalHandler
    {
        IReadOnlyAsyncReactiveProperty<bool> Revived { get; }
        Transform RevivePoint { get; }

        void Revive();
        void InitRevivePoint(Transform transform);
    }

    public class MainCharacterReviveGlobalHandler : IMainCharacterReviveGlobalHandler
    {
        private readonly AsyncReactiveProperty<bool> _revived = new(default);
        public Transform RevivePoint { get; private set; }

        public IReadOnlyAsyncReactiveProperty<bool> Revived => _revived;

        public void InitRevivePoint(Transform revivePoint)
        {
            RevivePoint = revivePoint;
        }

        public void Revive()
        {
            _revived.Value = true;
        }
    }
}
