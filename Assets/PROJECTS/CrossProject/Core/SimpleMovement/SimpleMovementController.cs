using System;
using System.Collections.Generic;
using CrossProject.Core.Camera;
using CrossProject.Core.Skins;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.AI;
using VContainer;
using VContainer.Unity;

namespace CrossProject.Core.SimpleMovement
{
    [RequireComponent(typeof(NavMeshAgent))]
    public class SimpleMovementController : MonoBehaviour, ITickable, IPostInitializable, IBlockable, IPlayerVelocityProvider
    {
        private CameraService _cameraService;
        private IJoystickValueProvider _joystick;

        //TODO : VM : move to skin controller
        [SerializeField] private Transform skinRoot;
        [SerializeField] private Skin defaultSkinPrefab;

        private NavMeshAgent _playerNavMeshAgent;
        private Vector3 _direction;
        private Skin _currentSkin;

        private static readonly int Speed = Animator.StringToHash("Speed");
        private readonly HashSet<Type> _blockers = new();

        public bool IsBlocked => _blockers.Count > 0;
        public Vector3 Velocity => _playerNavMeshAgent.velocity;
        public Vector3 Direction => _direction;

        public void RequestBlock(object blockRequester)
        {
            _blockers.Add(blockRequester.GetType());
        }

        public void ReleaseBlock(object blockRequester)
        {
            _blockers.Remove(blockRequester.GetType());
        }

        private void Awake()
        {
            _playerNavMeshAgent = GetComponent<NavMeshAgent>();
            _playerNavMeshAgent.updateRotation = true;
            DontDestroyOnLoad(this);
            SetSkin(defaultSkinPrefab);
        }

        [Inject]
        private void Construct(
            CameraService cameraService,
            IJoystickValueProvider joystickValueProvider)
        {
            _cameraService = cameraService;
            _joystick = joystickValueProvider;
        }

        //TODO : VM : move to skin controller
        public void SetSkin(Skin skinPrefab)
        {
            var count = skinRoot.childCount;
            if (count > 0)
                for (int i = count - 1; i >= 0; i--)
                    Destroy(skinRoot.GetChild(i).gameObject);

            _currentSkin = Instantiate(skinPrefab, skinRoot);
        }

        public void Tick()
        {
            if (!IsBlocked)
            {
                _direction = _cameraService.CamDirectionOnPlane.normalized;
                _direction.x *= _joystick.NormalizedVector2.x;
                //TODO : VM : fix later
                _direction.x *= 1.5f;
                _direction.z *= _joystick.NormalizedVector2.y;
                _playerNavMeshAgent.SetDestination(transform.position + _direction);
            }

            if (_currentSkin != null && _currentSkin.Animator != null)
                _currentSkin.Animator.SetFloat(Speed, _joystick.NormalizedVector2.sqrMagnitude > 0 ? 1 : 0);
        }

        public async UniTask MoveTo(Vector3 target, float sqrTargetDistance = 1)
        {
            _playerNavMeshAgent.SetDestination(target);
            await UniTask.WaitUntil(() => (_playerNavMeshAgent.transform.position - target).sqrMagnitude <= sqrTargetDistance);
        }

        public void PostInitialize()
        {
            _cameraService.SetTarget(transform);
        }
    }
}