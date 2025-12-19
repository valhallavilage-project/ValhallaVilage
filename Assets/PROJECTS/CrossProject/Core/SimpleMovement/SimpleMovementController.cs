using System;
using System.Collections.Generic;
using System.Threading;
using CrossProject.Core.Audio;
using CrossProject.Core.Camera;
using CrossProject.Core.Interactions;
using CrossProject.Core.Skins;
using CrossProject.Core.SpawnPoints;
using Cysharp.Threading.Tasks;
using Cysharp.Threading.Tasks.Linq;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.AI;
using VContainer;
using VContainer.Unity;

namespace CrossProject.Core.SimpleMovement
{
    [RequireComponent(typeof(NavMeshAgent))]
    public class SimpleMovementController : MonoBehaviour, ITickable, IInitializable, IBlockable, IPlayerVelocityProvider, IPlayerSkinProvider
    {
        private CameraService _cameraService;
        private IJoystickValueProvider _joystick;
        private SpawnPointService _spawnPointService;

        [SerializeField] private float speed = 3.5f;
        [SerializeField] private float acceleration = 10f;
        [SerializeField] private float deceleration = 10f;
        [SerializeField, Range(0, 180)] private float instantTurnAngle = 90f;
        [SerializeField] private Transform skinRoot;
        [SerializeField][FoldoutGroup("Sounds")] private AudioClip _steps;

        private NavMeshAgent _playerNavMeshAgent;
        private Transform _transform;
        private Vector3 _direction;
        private Vector3 _lastDirection;
        private Vector3 _currentVelocity;
        private Skin _skin;

        private static readonly int Speed = Animator.StringToHash("Speed");
        private readonly HashSet<Type> _blockers = new();
        private IMainCharacterMovingHandler _movingHandler;
        private bool _isIdling;

        public bool IsBlocked => _blockers.Count > 0;
        public Vector3 Velocity => _playerNavMeshAgent.velocity;
        public Vector3 Direction => _direction;
        public Transform PlayerSkinRoot => skinRoot;
        public bool IsInitialized { get; private set; }

        Skin IPlayerSkinProvider.CurrentSkin => _skin ??= skinRoot.GetComponentInChildren<Skin>();

        private Skin LocalAccessCurrentSkin => ((IPlayerSkinProvider)this).CurrentSkin;

        public void AddBlock(Type blockRequester)
        {
            _blockers.Add(blockRequester);
        }

        public void RemoveBlock(Type blockRequester)
        {
            _blockers.Remove(blockRequester);
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
            SpawnPointService spawnPointService,
            IMainCharacterGlobalArmorSetChangeHandler armorSetsService,
            IMainCharacterMovingHandler movingHandler,
            IInteractionHandler interactionHandler)
        {
            _movingHandler = movingHandler;
            _cameraService = cameraService;
            _joystick = joystickValueProvider;
            _spawnPointService = spawnPointService;
            
            armorSetsService.ArmorSetChanged.WithoutCurrent().ForEachAsync(ArmorSetChanged, gameObject.GetCancellationTokenOnDestroy()).Forget();
            interactionHandler.InteractionStarted.WithoutCurrent().ForEachAsync(InteractionStarted, gameObject.GetCancellationTokenOnDestroy()).Forget();
            interactionHandler.InteractionFinished.WithoutCurrent().ForEachAsync(InteractionFinished, gameObject.GetCancellationTokenOnDestroy()).Forget();
        }

        public void Tick()
        {
            if (IsBlocked)
                return;

            _direction = _cameraService.CamDirectionOnPlane.normalized;
            _direction.x *= _joystick.NormalizedVector2.x * 1.5f; //TODO : VM : fix later
            _direction.z *= _joystick.NormalizedVector2.y;
            var inputDir = _direction.normalized;

            var hasInput = inputDir != Vector3.zero;
            var targetSpeed = hasInput ? speed : 0;
            var currentSpeed = _currentVelocity.magnitude;
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

            if (currentSpeed == 0 && !_isIdling)
            {
                _isIdling = true;
                _movingHandler.StopMove();
            }
            else if (currentSpeed > 0 && _isIdling)
            {
                _isIdling = false;
                _movingHandler.BeginMove();
            }

            // Fix: Rotate character towards NavMesh path direction (no more moving backwards)
            if (_playerNavMeshAgent.hasPath && _playerNavMeshAgent.velocity.sqrMagnitude > 0.1f)
            {
                var moveDirection = _playerNavMeshAgent.velocity;
                moveDirection.y = 0;
                if (moveDirection != Vector3.zero)
                {
                    var targetRotation = Quaternion.LookRotation(moveDirection);

                    // Calculate angle difference for adaptive rotation speed
                    var angleDiff = Quaternion.Angle(_transform.rotation, targetRotation);

                    // DEBUG: Log rotation info
                    Debug.Log($"[NavMesh Rotation] hasPath={_playerNavMeshAgent.hasPath}, " +
                             $"velocity={_playerNavMeshAgent.velocity.magnitude:F2}, " +
                             $"angleDiff={angleDiff:F1}°, " +
                             $"currentRotation={_transform.rotation.eulerAngles.y:F1}°, " +
                             $"targetRotation={targetRotation.eulerAngles.y:F1}°");

                    // Adaptive rotation: sharp turns = instant, gradual turns = smooth
                    // If angle > 45 degrees (sharp turn) = instant rotation
                    // If angle < 45 degrees = smooth Slerp
                    var rotationSpeed = angleDiff > 45f ? 1f : Time.deltaTime * 20f;

                    _transform.rotation = Quaternion.Slerp(_transform.rotation, targetRotation, rotationSpeed);
                }
            }

            _playerNavMeshAgent.velocity = _currentVelocity;
            _playerNavMeshAgent.nextPosition = _transform.position + _currentVelocity * Time.deltaTime;
            _transform.position = _playerNavMeshAgent.nextPosition;

            if (LocalAccessCurrentSkin != null && LocalAccessCurrentSkin.Animator != null)
            {
                var animSpeed = Mathf.InverseLerp(0, speed, _currentVelocity.magnitude);
                LocalAccessCurrentSkin.Animator.SetFloat(Speed, animSpeed);
            }
        }

        public async UniTask MoveTo(Vector3 target, CancellationToken cancellationToken, float targetDistance = 1)
        {
            var direction = target - _transform.position;
            if (direction != Vector3.zero)
            {
                _transform.forward = direction;
                _isIdling = false;
            }

            _playerNavMeshAgent.SetDestination(target);
            LocalAccessCurrentSkin.Animator.SetFloat(Speed, 1);
            await UniTask.WaitUntil(() => !_playerNavMeshAgent.pathPending &&
                                          _playerNavMeshAgent.remainingDistance <= targetDistance, PlayerLoopTiming.Update, cancellationToken);
            _playerNavMeshAgent.ResetPath();
            _playerNavMeshAgent.velocity = Vector3.zero;
            _currentVelocity = Vector3.zero;
            LocalAccessCurrentSkin.Animator.SetFloat(Speed, 0);
        }

        public async UniTask Initialize()
        {
            await UniTask.WaitUntil(() => _cameraService.IsInitialized && _spawnPointService.IsInitialized);
            _cameraService.SetTarget(_transform);
            var targetPos = _spawnPointService.GetPosition(new SpawnPointId("PlayerSpawnPoint"));
            _playerNavMeshAgent.Warp(targetPos);
            IsInitialized = true;
        }

        private void ArmorSetChanged(MainCharacterArmorSetType armorSet)
        {
            LocalAccessCurrentSkin.SelectSet(armorSet);
        }

        private void InteractionStarted(InteractionType interaction)
        {
            LocalAccessCurrentSkin.ActivateTool(interaction);
        }

        private void InteractionFinished(InteractionType interaction)
        {
            LocalAccessCurrentSkin.DeactivateTool();
        }
    }
}