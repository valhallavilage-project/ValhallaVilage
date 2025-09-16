using Cysharp.Threading.Tasks;
using UnityEngine;

namespace CrossProject.Core
{
    public interface IRotateAbility
    {
        IReadOnlyAsyncReactiveProperty<Vector3> Direction { get; }
        IReadOnlyAsyncReactiveProperty<Vector3> ForceDirection { get; }
        IReadOnlyAsyncReactiveProperty<bool> Stop { get; }
        RotateAbilityParameters Parameters { get; }

        void Init(float rotationSpeed, float rotationDamper);
        void Rotate(Vector3 direction);
        void StopRotation();
        void ForceRotate(Vector3 direction);
    }

    public class RotateAbility : IRotateAbility
    {
        private readonly AsyncReactiveProperty<Vector3> _direction = new(default);
        private readonly AsyncReactiveProperty<bool> _stop = new(default);
        private readonly AsyncReactiveProperty<Vector3> _forceDirection = new(default);

        public IReadOnlyAsyncReactiveProperty<Vector3> Direction => _direction;
        public IReadOnlyAsyncReactiveProperty<bool> Stop => _stop;
        public IReadOnlyAsyncReactiveProperty<Vector3> ForceDirection => _forceDirection;

        public RotateAbilityParameters Parameters { get; private set; }

        public void Init(float rotationSpeed, float rotationDamper)
        {
            Parameters = new RotateAbilityParameters(rotationSpeed, rotationDamper);
        }

        public void Rotate(Vector3 direction)
        {
            _direction.Value = direction;
        }

        public void ForceRotate(Vector3 direction)
        {
            _forceDirection.Value = direction;
        }

        public void StopRotation()
        {
            _stop.Value = true;
        }
    }
}