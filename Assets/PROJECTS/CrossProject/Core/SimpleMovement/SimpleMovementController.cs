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

        [SerializeField] private float speed = 3.5f;
        [SerializeField] private float acceleration = 10f;
        [SerializeField] private float deceleration = 10f;
        [SerializeField, Range(0, 180)] private float instantTurnAngle = 90f;
        [SerializeField] private Transform skinRoot;

        private NavMeshAgent _playerNavMeshAgent;
        private Transform _transform;
        private Vector3 _direction;
        private Vector3 _lastDirection;
        private Vector3 _currentVelocity;
        private Skin _skin;

        private static readonly int Speed = Animator.StringToHash("Speed");
        private readonly HashSet<Type> _blockers = new();

        public bool IsBlocked => _blockers.Count > 0;
        public Vector3 Velocity => _playerNavMeshAgent.velocity;
        public Vector3 Direction => _direction;
        public Transform PlayerSkinRoot => skinRoot;

        Skin IPlayerSkinProvider.CurrentSkin => _skin ??= skinRoot.GetComponentInChildren<Skin>();

        private Skin LocalAccessCurrentSkin => ((IPlayerSkinProvider)this).CurrentSkin;

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
            _playerNavMeshAgent.updateRotation = false;
            _transform = transform;
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
            _direction.x *= _joystick.NormalizedVector2.x * 1.5f; //TODO : VM : fix later
            _direction.z *= _joystick.NormalizedVector2.y;
            var inputDir = _direction.normalized;

            bool hasInput = inputDir != Vector3.zero;
            float targetSpeed = hasInput ? speed : 0;
            float currentSpeed = _currentVelocity.magnitude;
            currentSpeed = Mathf.MoveTowards(currentSpeed, targetSpeed, (hasInput ? acceleration : deceleration)*Time.deltaTime);

            if (!hasInput)
            {
                _currentVelocity = _transform.forward * currentSpeed;
            }
            else
            {
                _transform.forward = inputDir;
                _currentVelocity = inputDir * currentSpeed;
            }

            _playerNavMeshAgent.velocity = _currentVelocity;
            _playerNavMeshAgent.nextPosition = _transform.position + _currentVelocity * Time.deltaTime;
            _transform.position = _playerNavMeshAgent.nextPosition;

            if (LocalAccessCurrentSkin != null && LocalAccessCurrentSkin.Animator != null)
            {
                float animSpeed = Mathf.InverseLerp(0, speed, _currentVelocity.magnitude);
                LocalAccessCurrentSkin.Animator.SetFloat(Speed, animSpeed);
            }
        }

        public async UniTask MoveTo(Vector3 target, CancellationToken cancellationToken, float targetDistance = 1)
        {
            var direction = target - _transform.position;
            if (direction != Vector3.zero)
                _transform.forward = direction;
            _playerNavMeshAgent.SetDestination(target);
            LocalAccessCurrentSkin.Animator.SetFloat(Speed, 1);
            await UniTask.WaitUntil(() => (_transform.position - target).sqrMagnitude <= targetDistance * targetDistance, PlayerLoopTiming.Update, cancellationToken);
            LocalAccessCurrentSkin.Animator.SetFloat(Speed, 0);
        }

        public void PostInitialize()
        {
            _cameraService.SetTarget(_transform);
            var targetPos = _spawnPointService.GetPosition(new SpawnPointId("PlayerSpawnPoint"));
            _playerNavMeshAgent.Warp(targetPos);
        }
    }
}