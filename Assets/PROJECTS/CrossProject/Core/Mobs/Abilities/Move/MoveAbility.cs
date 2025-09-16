using Cysharp.Threading.Tasks;
using UnityEngine;

namespace CrossProject.Core
{
    public interface IMoveAbility
    {
        IReadOnlyAsyncReactiveProperty<Velocity> Velocity { get; }
        IReadOnlyAsyncReactiveProperty<bool> Stop { get; }
        MoveAbilityParameters Parameters { get; }

        void Init(float acceleration, float maxAcceleration);
        void Move(Vector3 direction, float speed);
        void StopMovement();
    }

    public class MoveAbility : IMoveAbility
    {
        private readonly AsyncReactiveProperty<Velocity> _velocity = new(default);
        private readonly AsyncReactiveProperty<bool> _stop = new(default);

        public MoveAbilityParameters Parameters { get; private set; }
        public IReadOnlyAsyncReactiveProperty<Velocity> Velocity => _velocity;
        public IReadOnlyAsyncReactiveProperty<bool> Stop => _stop;

        public void Init(float acceleration, float maxAcceleration)
        {
            Parameters = new MoveAbilityParameters(acceleration, maxAcceleration);
        }

        public void Move(Vector3 direction, float speed)
        {
            _velocity.Value = new Velocity(direction, speed);
        }

        public void StopMovement()
        {
            _stop.Value = true;
        }
    }
}