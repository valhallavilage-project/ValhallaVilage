using System.Collections;
using CrossProject.Core.Interactions;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.AI;
using Random = UnityEngine.Random;

namespace L2Farm.Scripts.Mobs
{
    public class Mob : InteractiveObject
    {
        public float minRadius = 5f;
        public float maxRadius = 15f;
        public float minWaitTime = 1f;
        public float maxWaitTime = 5f;
        public float maxMoveTime = 10f;
        public float reachDistance = 1.5f;

        [SerializeField] private Animator animator;
        [SerializeField] private NavMeshAgent agent;

        private Coroutine _walkCoroutine;
        private bool _isMoving;
        private readonly int Speed = Animator.StringToHash("Speed");

        private void Start()
        {
            _walkCoroutine = StartCoroutine(WalkRoutine());
        }

        private IEnumerator WaitForArrival(Vector3 target)
        {
            float elapsedTime = 0f;

            while (elapsedTime < maxMoveTime)
            {
                if (!agent.pathPending)
                {
                    if (agent.remainingDistance <= reachDistance)
                    {
                        _isMoving = false;
                        yield break;
                    }

                    if (agent.isOnNavMesh && !agent.hasPath && !agent.pathPending)
                    {
                        _isMoving = false;
                        yield break;
                    }
                }

                elapsedTime += Time.deltaTime;
                yield return null;
            }

            _isMoving = false;
            agent.ResetPath();
        }

        private IEnumerator WalkRoutine()
        {
            while (true)
            {
                Vector3 randomPoint = GetRandomPointOnNavMesh(transform.position, 
                    Random.Range(minRadius, maxRadius));

                if (randomPoint != Vector3.zero)
                {
                    agent.SetDestination(randomPoint);
                    _isMoving = true;

                    animator?.SetFloat(Speed, 100);
                    yield return StartCoroutine(WaitForArrival(randomPoint));
                    animator?.SetFloat(Speed, 0);

                    float waitTime = Random.Range(minWaitTime, maxWaitTime);
                    yield return new WaitForSeconds(waitTime);
                }
                else
                {
                    yield return new WaitForSeconds(1f);
                }
            }
        }

        private Vector3 GetRandomPointOnNavMesh(Vector3 center, float radius)
        {
            Vector3 randomPoint = center + Random.insideUnitSphere * radius;
            NavMeshHit hit;

            if (NavMesh.SamplePosition(randomPoint, out hit, radius, NavMesh.AllAreas))
            {
                return hit.position;
            }

            if (NavMesh.SamplePosition(center, out hit, 1f, NavMesh.AllAreas))
            {
                return hit.position;
            }

            return Vector3.zero;
        }

        private void OnDisable()
        {
            if (_walkCoroutine != null)
                StopCoroutine(_walkCoroutine);
        }

        public void StopWalking()
        {
            if (_walkCoroutine != null)
            {
                StopCoroutine(_walkCoroutine);
                agent.ResetPath();
            }
        }

        public void ResumeWalking()
        {
            if (_walkCoroutine != null)
                StopCoroutine(_walkCoroutine);
        
            _walkCoroutine = StartCoroutine(WalkRoutine());
        }
        public override async UniTask Interaction()
        {
            Debug.Log("Attack Enemy");
            await UniTask.CompletedTask;
        }
    }
}
