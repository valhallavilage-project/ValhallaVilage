using CrossProject.Core.Pooling;
using Cysharp.Threading.Tasks;
using Sirenix.OdinInspector;
using UnityEngine;
using VContainer;

namespace CrossProject.Core
{
    public class MobView : MonoBehaviour, IPoolElement
    {
        [SerializeField] [ReadOnly] private MobState _state;
        [SerializeField] private Rigidbody _rigidbody;
        [SerializeField] private GameObject _spawnFx;
        

        private MobsSpawnPoint _spawnPoint;
        private IMobStateMachine _mobStateMachine;
        private IRoamArea _roamArea;
        private IHealthHandler _healthHandler;

        private IMobPerUpdateData _perUpdateData;
        private MobConfig _config;

        private IMoveAbility _moveAbility;
        private IRotateAbility _rotateAbility;
        private IPool _pool;
        private IAgroArea _agroArea;

        public bool IsAvailableToGet { get; private set; }

        [Inject]
        public void AddDependencies(IMobStateMachine stateMachine, IMobPerUpdateData perUpdateData,
            MobConfig config, IMoveAbility moveAbility, IRotateAbility rotateAbility, IRoamArea roamArea,
            IHealthHandler healthHandler, IDieAbility dieAbility, IAgroArea agroArea)
        {
            _perUpdateData = perUpdateData;
            _config = config;
            _roamArea = roamArea;
            _healthHandler = healthHandler;

            _mobStateMachine = stateMachine;

            _moveAbility = moveAbility;
            _rotateAbility = rotateAbility;
            _agroArea = agroArea;

            healthHandler.Init(config.Health, config.Health);
            moveAbility.Init(config.Acceleration, config.MaxAcceleration);
            rotateAbility.Init(config.RotationSpeed, config.RotationDamper);
            dieAbility.DeathCompleted.Listen(MobDie, gameObject.GetCancellationTokenOnDestroy());
        }

        private void FixedUpdate()
        {
            _state = _mobStateMachine.CurrentState.Value;
            
            #if UNITY_EDITOR
            // to apply changes made in config during play mode
            _moveAbility.Init(_config.Acceleration, _config.MaxAcceleration);
            _rotateAbility.Init(_config.RotationSpeed, _config.RotationDamper);
            #endif

            _perUpdateData.LinearVelocity = _rigidbody.velocity;
            _perUpdateData.AngularVelocity = _rigidbody.angularVelocity;
            _perUpdateData.Rotation = transform.rotation;
            _perUpdateData.Position = transform.position;

            _mobStateMachine.Execute();
        }

        public void SetPool(IPool pool)
        {
            _pool = pool;
        }

        public void OnGet()
        {
            gameObject.SetActive(true);
            IsAvailableToGet = false;
            _healthHandler.Init(_config.Health, _config.Health);
            _mobStateMachine.ToDefaultState();
            _spawnFx.SetActive(true);
        }

        public void OnReturn()
        {
            gameObject.SetActive(false);
            _spawnPoint.RemoveMob();
            IsAvailableToGet = true;
            _spawnFx.SetActive(false);
        }

        public void BindSpawnPoint(MobsSpawnPoint spawnPoint)
        {
            _spawnPoint = spawnPoint;
            _roamArea.Init(spawnPoint.RoamZone, _config.RoamingMinPathLength);
            _agroArea.Init(spawnPoint.AgroZone);
        }

        private void MobDie()
        {
            _pool.Return(this);
        }
    }
}
