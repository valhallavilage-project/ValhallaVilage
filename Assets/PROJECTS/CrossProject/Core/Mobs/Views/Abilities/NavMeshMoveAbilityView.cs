using Cysharp.Threading.Tasks;
using Cysharp.Threading.Tasks.Linq;
using UnityEngine;
using UnityEngine.AI;
using VContainer;

namespace CrossProject.Core
{
    /// <summary>
    /// NavMesh-based movement for mobs. Replaces physics-based MoveAbilityView.
    /// Uses NavMeshAgent for pathfinding and obstacle avoidance.
    /// </summary>
    [RequireComponent(typeof(NavMeshAgent))]
    public class NavMeshMoveAbilityView : MonoBehaviour
    {
        [SerializeField] private NavMeshAgent _agent;
        [SerializeField] private Rigidbody _rigidbody;

        private IMoveAbility _moveAbility;
        private Vector3 _targetDirection;
        private float _targetSpeed;
        private bool _isStopped = true;

        [Inject]
        public void AddDependencies(IMoveAbility moveAbility)
        {
            _moveAbility = moveAbility;

            moveAbility.Velocity.WithoutCurrent().ForEachAsync(VelocityChanged, gameObject.GetCancellationTokenOnDestroy()).Forget();
            moveAbility.Stop.WithoutCurrent().ForEachAsync(Stop, gameObject.GetCancellationTokenOnDestroy()).Forget();
        }

        private void Awake()
        {
            if (_agent == null)
            {
                _agent = GetComponent<NavMeshAgent>();
            }

            if (_rigidbody == null)
            {
                _rigidbody = GetComponent<Rigidbody>();
            }

            // NavMeshAgent will handle movement, disable Rigidbody physics movement
            if (_rigidbody != null)
            {
                _rigidbody.isKinematic = true;
            }

            // Configure agent for mob behavior
            _agent.updateRotation = false; // We handle rotation via RotateAbility
            _agent.updateUpAxis = false;
        }

        private void VelocityChanged(Velocity velocity)
        {
            if (velocity.Direction == Vector3.zero || velocity.Speed <= 0)
            {
                StopAgent();
                return;
            }

            _targetDirection = velocity.Direction;
            _targetSpeed = velocity.Speed;
            _isStopped = false;

            // Set agent speed
            _agent.speed = _targetSpeed;
            _agent.isStopped = false;

            // Calculate target position in movement direction
            var targetPosition = transform.position + _targetDirection * 10f;

            // Sample valid NavMesh position
            if (NavMesh.SamplePosition(targetPosition, out var hit, 15f, NavMesh.AllAreas))
            {
                _agent.SetDestination(hit.position);
            }
            else
            {
                // If no valid NavMesh point, try shorter distance
                targetPosition = transform.position + _targetDirection * 2f;
                if (NavMesh.SamplePosition(targetPosition, out hit, 5f, NavMesh.AllAreas))
                {
                    _agent.SetDestination(hit.position);
                }
            }
        }

        private void Stop(bool _)
        {
            StopAgent();
        }

        private void StopAgent()
        {
            _isStopped = true;
            if (_agent.isOnNavMesh)
            {
                _agent.isStopped = true;
                _agent.velocity = Vector3.zero;
            }
        }

        private void OnEnable()
        {
            // Warp agent to current position when enabled (for pooling)
            if (_agent != null && NavMesh.SamplePosition(transform.position, out var hit, 5f, NavMesh.AllAreas))
            {
                _agent.Warp(hit.position);
            }
        }
    }
}
