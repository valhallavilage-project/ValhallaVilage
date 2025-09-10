using Cysharp.Threading.Tasks;
using Cysharp.Threading.Tasks.Linq;
using UnityEngine;
using VContainer;

namespace CrossProject.Core
{
    public class RotateAbilityView : MonoBehaviour
    {
        private const RigidbodyConstraints ConstraintsMask = RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationY | RigidbodyConstraints.FreezeRotationZ;

        [SerializeField] private Rigidbody _rigidbody;

        private IRotateAbility _rotateAbility;
        private RigidbodyConstraints _initialConstraints;

        [Inject]
        public void AddDependencies(IRotateAbility rotateAbility)
        {
            _rotateAbility = rotateAbility;

            rotateAbility.Direction.WithoutCurrent().ForEachAsync(DirectionChanged).Forget();
            rotateAbility.Stop.WithoutCurrent().ForEachAsync(StopRotation).Forget();
            rotateAbility.ForceDirection.WithoutCurrent().ForEachAsync(ForceRotate).Forget();
        }

        private void Start()
        {
            _initialConstraints = _rigidbody.constraints & ConstraintsMask;
        }

        private void DirectionChanged(Vector3 direction)
        {
            var rotation = GetRotation(direction, transform.rotation, _rigidbody.angularVelocity,
                _rotateAbility.Parameters.RotationSpeed, _rotateAbility.Parameters.RotationDamper);

            if (_rotateAbility.Parameters.RotationSpeed > 0)
            {
                var otherConstraints = _rigidbody.constraints & ~ConstraintsMask;

                _rigidbody.constraints = otherConstraints | _initialConstraints;
            }

            _rigidbody.AddTorque(rotation*_rigidbody.mass);
        }

        private void StopRotation(bool _)
        {
            _rigidbody.constraints |= ConstraintsMask;
        }

        private void ForceRotate(Vector3 direction)
        {
            _rigidbody.angularVelocity = Vector3.zero;
            _rigidbody.transform.rotation = Quaternion.LookRotation(direction);
        }

        private Vector3 GetRotation(Vector3 direction, Quaternion currentRotation, Vector3 angularVelocity, float rotationSpeed, float rotationDamper)
        {
            var rotation = Quaternion.LookRotation(direction);
            var targetRotation = Quaternion.Slerp(currentRotation, rotation, MathUtils.GetInterpolant(rotationSpeed, Time.fixedDeltaTime));

            MathUtils.GetShortestRotation(targetRotation, currentRotation).ToAngleAxis(out var rotationAngle, out var rotationAxis);

            return rotationAxis.normalized*rotationAngle*Mathf.Deg2Rad - (new Vector3(0, angularVelocity.y, 0)*rotationDamper);
        }
    }
}
