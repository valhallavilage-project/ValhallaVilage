using System;
using System.Collections.Generic;
using System.Threading;
using CrossProject.Core.Camera;
using CrossProject.Core.Skins;
using CrossProject.Core.SpawnPoints;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.AI;
using VContainer;
using VContainer.Unity;

namespace CrossProject.Core.SimpleMovement
{
    [RequireComponent(typeof(NavMeshAgent))]
    public class SimpleMovementController : MonoBehaviour, ITickable, IPostInitializable, IBlockable, IPlayerVelocityProvider, IPlayerSkinProvider
    {
        private CameraService _cameraService;
        private IJoystickValueProvider _joystick;
        private SpawnPointService _spawnPointService;

        [SerializeField] private Transform skinRoot;

        private NavMeshAgent _playerNavMeshAgent;
        private Vector3 _direction;
        private Skin _skin;

        private static readonly int Speed = Animator.StringToHash("Speed");
        private readonly HashSet<Type> _blockers = new();

        public bool IsBlocked => _blockers.Count > 0;
        public Vector3 Velocity => _playerNavMeshAgent.velocity;
        public Vector3 Direction => _direction;
        public Transform PlayerSkinRoot => skinRoot;

        private Skin CurrentSkin => _skin ??= skinRoot.GetComponentInChildren<Skin>();

        public void AddBlock(object blockRequester)
        {
            _blockers.Add(blockRequester.GetType());
        }

        public void RemoveBlock(object blockRequester)
        {
            _blockers.Remove(blockRequester.GetType());
        }

        private void Awake()
        {
            _playerNavMeshAgent = GetComponent<NavMeshAgent>();
            _playerNavMeshAgent.updateRotation = true;
            DontDestroyOnLoad(this);
        }

        [Inject]
        private void Construct(
            CameraService cameraService,
            IJoystickValueProvider joystickValueProvider,
            SpawnPointService spawnPointService)
        {
            _cameraService = cameraService;
            _joystick = joystickValueProvider;
            _spawnPointService = spawnPointService;
        }

        public void Tick()
        {
            if (IsBlocked)
                return;

            _direction = _cameraService.CamDirectionOnPlane.normalized;
            _direction.x *= _joystick.NormalizedVector2.x;
            //TODO : VM : fix later
            _direction.x *= 1.5f;
            _direction.z *= _joystick.NormalizedVector2.y;
            _playerNavMeshAgent.isStopped = false;
            _playerNavMeshAgent.SetDestination(transform.position + _direction);

            if (CurrentSkin != null && CurrentSkin.Animator != null)
            {
                var velocity = _playerNavMeshAgent.velocity.magnitude;
                var animSpeed = Mathf.InverseLerp(0, _playerNavMeshAgent.speed, velocity);
                CurrentSkin.Animator.SetFloat(Speed, animSpeed);
            }
        }

        public async UniTask MoveTo(Vector3 target, CancellationToken cancellationToken, float targetDistance = 1)
        {
            _playerNavMeshAgent.SetDestination(target);
            CurrentSkin.Animator.SetFloat(Speed, 100);
            await UniTask.WaitUntil(() => (transform.position - target).sqrMagnitude <= targetDistance * targetDistance, PlayerLoopTiming.Update, cancellationToken);
            CurrentSkin.Animator.SetFloat(Speed, 0);
            _playerNavMeshAgent.isStopped = true;
        }

        public void PostInitialize()
        {
            _cameraService.SetTarget(transform);
            var targetPos = _spawnPointService.GetPosition(new SpawnPointId("PlayerSpawnPoint"));
            _playerNavMeshAgent.Warp(targetPos);
        }
    }
}