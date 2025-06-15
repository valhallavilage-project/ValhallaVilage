using CrossProject.Core.Camera;
using UnityEngine;
using UnityEngine.AI;
using VContainer.Unity;

namespace CrossProject.Core.SimpleMovement
{
    [RequireComponent(typeof(NavMeshAgent))]
    public class SimpleMovementController : MonoBehaviour, IPostInitializable, ITickable
    {
        private readonly CameraService _cameraService;
        private readonly IJoystickValueProvider _joystick;

        private Transform _playerRoot;
        private NavMeshAgent _playerNavMeshAgent;

        public SimpleMovementController(
            CameraService cameraService,
            IJoystickValueProvider joystick)
        {
            _cameraService = cameraService;
            _joystick = joystick;
        }

        private void Awake()
        {
            _playerRoot = transform;
            _playerNavMeshAgent = GetComponent<NavMeshAgent>();
        }

        public void PostInitialize()
        {
            _cameraService.SetTarget(_playerRoot);
        }

        public void Tick()
        {
            _playerNavMeshAgent.SetDestination(_playerRoot.position + _joystick.NormalizedValueProjectOnPlane);
        }
    }
}