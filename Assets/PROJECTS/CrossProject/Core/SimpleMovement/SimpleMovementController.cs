using CrossProject.Core.Camera;
using CrossProject.Core.Skins;
using UnityEngine;
using UnityEngine.AI;
using VContainer;
using VContainer.Unity;

namespace CrossProject.Core.SimpleMovement
{
    [RequireComponent(typeof(NavMeshAgent))]
    public class SimpleMovementController : MonoBehaviour, ITickable, IPostInitializable
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

        private void Awake()
        {
            _playerNavMeshAgent = GetComponent<NavMeshAgent>();
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
            _direction = _cameraService.CamDirectionOnPlane.normalized;
            _direction.z *= _joystick.NormalizedVector2.y;
            _direction.x *= _joystick.NormalizedVector2.x;
            _playerNavMeshAgent.SetDestination(transform.position + _direction);
            if (_currentSkin != null && _currentSkin.Animator != null)
                _currentSkin.Animator.SetFloat(Speed, _playerNavMeshAgent.speed * _direction.sqrMagnitude);
        }

        public void PostInitialize()
        {
            _cameraService.SetTarget(transform);
        }
    }
}