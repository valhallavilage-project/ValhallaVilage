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
        private Vector3 _lastPosition;
        private float _stuckTimer;

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

            // Apply obstacle sliding - project movement along obstacles
            if (hasInput && currentSpeed > 0)
            {
                _currentVelocity = GetSlideVelocity(_currentVelocity);
            }

            _playerNavMeshAgent.velocity = _currentVelocity;
            _playerNavMeshAgent.nextPosition = _transform.position + _currentVelocity * Time.deltaTime;
            _transform.position = _playerNavMeshAgent.nextPosition;

            // Check actual movement (not just intended velocity)
            var actualDelta = (_transform.position - _lastPosition).sqrMagnitude;
            var intendedMovement = _currentVelocity.sqrMagnitude > 0.01f;
            var actuallyMoving = actualDelta > 0.0001f; // Actually moved this frame

            // Track stuck state - if we want to move but can't
            if (intendedMovement && !actuallyMoving)
            {
                _stuckTimer += Time.deltaTime;
            }
            else
            {
                _stuckTimer = 0f;
            }

            // Consider stuck after 0.1 seconds of no movement while trying to move
            var isStuck = _stuckTimer > 0.1f;

            if ((isStuck || !intendedMovement) && !_isIdling)
            {
                _isIdling = true;
                _movingHandler.StopMove();
            }
            else if (!isStuck && intendedMovement && _isIdling)
            {
                _isIdling = false;
                _movingHandler.BeginMove();
            }

            _lastPosition = _transform.position;

            if (LocalAccessCurrentSkin != null && LocalAccessCurrentSkin.Animator != null)
            {
                // Use 0 speed when stuck
                var animSpeed = isStuck ? 0f : Mathf.InverseLerp(0, speed, _currentVelocity.magnitude);
                LocalAccessCurrentSkin.Animator.SetFloat(Speed, animSpeed);
            }
        }

        private Vector3 GetSlideVelocity(Vector3 desiredVelocity)
        {
            var moveDir = desiredVelocity.normalized;
            var checkDistance = 0.8f;
            var sphereRadius = 0.4f;
            var origin = _transform.position + Vector3.up * 0.5f;

            // Use Physics.AllLayers to check everything, ignore triggers
            var queryTrigger = QueryTriggerInteraction.Ignore;
            var layerMask = ~0; // All layers

            // Try multiple rays for better detection
            var hitDetected = false;
            RaycastHit bestHit = default;

            // Center ray
            if (Physics.Raycast(origin, moveDir, out var centerHit, checkDistance, layerMask, queryTrigger))
            {
                if (!IsOwnCollider(centerHit.collider))
                {
                    hitDetected = true;
                    bestHit = centerHit;
                }
            }

            // SphereCast as backup
            if (!hitDetected && Physics.SphereCast(origin, sphereRadius, moveDir, out var sphereHit, checkDistance, layerMask, queryTrigger))
            {
                if (!IsOwnCollider(sphereHit.collider))
                {
                    hitDetected = true;
                    bestHit = sphereHit;
                }
            }

            if (!hitDetected)
            {
                return desiredVelocity;
            }

            // Hit something - calculate slide direction
            var normal = bestHit.normal;
            normal.y = 0;
            if (normal.sqrMagnitude < 0.01f)
            {
                return Vector3.zero;
            }

            normal.Normalize();
            var slideDir = Vector3.ProjectOnPlane(moveDir, normal).normalized;

            if (slideDir.sqrMagnitude > 0.01f)
            {
                // Check if slide direction is clear
                if (!Physics.Raycast(origin, slideDir, checkDistance, layerMask, queryTrigger))
                {
                    var slideDot = Vector3.Dot(moveDir, slideDir);
                    return slideDir * desiredVelocity.magnitude * Mathf.Max(slideDot, 0.4f);
                }
            }

            // Stuck - stop movement completely
            return Vector3.zero;
        }

        private bool IsOwnCollider(Collider collider)
        {
            return collider.transform.IsChildOf(_transform) || collider.transform == _transform;
        }

        public async UniTask MoveTo(Vector3 target, CancellationToken cancellationToken, float targetDistance = 1)
        {
            // Rotate towards target immediately
            var direction = target - _transform.position;
            if (direction != Vector3.zero)
            {
                direction.y = 0;
                _transform.forward = direction.normalized;
                _isIdling = false;
            }

            _playerNavMeshAgent.SetDestination(target);

            // Wait for path calculation first (don't animate yet!)
            await UniTask.WaitUntil(() => !_playerNavMeshAgent.pathPending,
                PlayerLoopTiming.Update, cancellationToken);

            // Check if path is valid
            if (_playerNavMeshAgent.pathStatus == NavMeshPathStatus.PathInvalid)
            {
                return;
            }

            // Move and animate based on actual velocity
            while (!cancellationToken.IsCancellationRequested &&
                   _playerNavMeshAgent.remainingDistance > targetDistance)
            {
                var actualSpeed = _playerNavMeshAgent.velocity.magnitude;
                var animSpeed = Mathf.InverseLerp(0, speed, actualSpeed);
                LocalAccessCurrentSkin.Animator.SetFloat(Speed, animSpeed);

                // Rotate towards actual movement direction
                if (_playerNavMeshAgent.velocity.sqrMagnitude > 0.1f)
                {
                    var moveDir = _playerNavMeshAgent.velocity;
                    moveDir.y = 0;
                    if (moveDir != Vector3.zero)
                    {
                        _transform.rotation = Quaternion.LookRotation(moveDir);
                    }
                }
                await UniTask.Yield(PlayerLoopTiming.Update, cancellationToken);
            }

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