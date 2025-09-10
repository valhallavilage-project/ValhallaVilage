
using Cysharp.Threading.Tasks;
using Cysharp.Threading.Tasks.Linq;
using UnityEngine;
using VContainer;

namespace CrossProject.Core
{
    public class MoveAbilityView : MonoBehaviour
    {
        private const RigidbodyConstraints ConstraintsMask = RigidbodyConstraints.FreezePositionX | RigidbodyConstraints.FreezePositionY | RigidbodyConstraints.FreezePositionZ;

        [SerializeField] private Rigidbody _rigidbody;

        private Vector3 _goalVelocity;
        private IMoveAbility _moveAbility;
        private RigidbodyConstraints _initialConstraints;


        [Inject]
        public void AddDependencies(IMoveAbility moveAbility)
        {
            _moveAbility = moveAbility;

            moveAbility.Velocity.WithoutCurrent().ForEachAsync(VelocityChanged, gameObject.GetCancellationTokenOnDestroy()).Forget();
            moveAbility.Stop.WithoutCurrent().ForEachAsync(Stop, gameObject.GetCancellationTokenOnDestroy()).Forget();
        }

        private void Awake()
        {
            _initialConstraints = _rigidbody.constraints & ConstraintsMask;
        }

        private void VelocityChanged(Velocity velocity)
        {
            var acceleration = GetAcceleration(velocity, _rigidbody.velocity, _moveAbility.Parameters.Acceleration, _moveAbility.Parameters.MaxAcceleration);

            if (_moveAbility.Parameters.Acceleration > 0)
            {
                var otherConstraints = _rigidbody.constraints & ~ConstraintsMask;

                _rigidbody.constraints = otherConstraints | _initialConstraints;
            }

            _rigidbody.AddForce(acceleration, ForceMode.Acceleration);
        }

        private Vector3 GetAcceleration(Velocity velocity, Vector3 currentVelocity, float acceleration, float maxAcceleration)
        {
            if (velocity.Direction == Vector3.zero)
            {
                return Vector3.zero;
            }

            var moveVelocity = velocity.Value;

            _goalVelocity = Vector3.MoveTowards(_goalVelocity, moveVelocity, acceleration*Time.fixedDeltaTime);

            var neededAcceleration = (_goalVelocity - currentVelocity)/Time.fixedDeltaTime;

            return Vector3.ClampMagnitude(Vector3.Scale(neededAcceleration, new Vector3(1, 0, 1)), maxAcceleration);
        }

        private void Stop(bool _)
        {
            _rigidbody.constraints |= ConstraintsMask;
        }
    }
}
