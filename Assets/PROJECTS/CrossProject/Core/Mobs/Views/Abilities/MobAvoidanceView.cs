using UnityEngine;

namespace CrossProject.Core
{
    /// <summary>
    /// Simple avoidance system - mobs push away from each other when too close.
    /// This prevents mobs from stacking on top of each other.
    /// </summary>
    public class MobAvoidanceView : MonoBehaviour
    {
        private const string MobTag = "Mob";

        [SerializeField] private Rigidbody _rigidbody;
        [SerializeField] private float _avoidanceRadius = 5f;
        [SerializeField] private float _avoidanceForce = 20f;
        [SerializeField] private float _minSeparation = 2f; // Minimum distance mobs try to keep

        private readonly Collider[] _nearbyColliders = new Collider[20];

        private void Awake()
        {
            if (_rigidbody == null)
            {
                _rigidbody = GetComponent<Rigidbody>();
            }
        }

        private void FixedUpdate()
        {
            if (_rigidbody == null) return;

            var position = transform.position;
            var count = Physics.OverlapSphereNonAlloc(position, _avoidanceRadius, _nearbyColliders);

            if (count == 0) return;

            var avoidanceDirection = Vector3.zero;

            for (var i = 0; i < count; i++)
            {
                var other = _nearbyColliders[i];

                // Skip non-mob objects
                if (!other.CompareTag(MobTag)) continue;

                // Skip self
                if (other.attachedRigidbody == _rigidbody) continue;

                var otherPosition = other.transform.position;
                var toThis = position - otherPosition;
                toThis.y = 0;
                var distance = toThis.magnitude;

                // Skip if distance is basically zero
                if (distance < 0.01f)
                {
                    // Random direction if completely overlapping
                    toThis = new Vector3(Random.Range(-1f, 1f), 0, Random.Range(-1f, 1f)).normalized;
                    distance = 0.01f;
                }

                // Strong repulsion when closer than minimum separation
                if (distance < _minSeparation)
                {
                    // Very strong force when too close - exponential increase
                    var urgency = Mathf.Pow((_minSeparation - distance) / _minSeparation, 2);
                    avoidanceDirection += toThis.normalized * (1f + urgency * 3f);
                }
                else if (distance < _avoidanceRadius)
                {
                    // Gentle push when in avoidance radius but not too close
                    var strength = 1f - (distance / _avoidanceRadius);
                    avoidanceDirection += toThis.normalized * strength * 0.5f;
                }
            }

            if (avoidanceDirection.sqrMagnitude > 0.001f)
            {
                _rigidbody.AddForce(avoidanceDirection * _avoidanceForce, ForceMode.Acceleration);
            }
        }
    }
}
