using CrossProject.Core.Pooling;
using Sirenix.OdinInspector;
using UnityEngine;
using VContainer;

namespace CrossProject.Core
{
    public class Mob : MonoBehaviour, IPoolElement
    {
        [SerializeField] private Rigidbody _rigidbody;
        [SerializeField] [ReadOnly] private MobState _state;

        private MobsSpawnPoint _spawnPoint;
        private IMobStateMachine _mobStateMachine;
        private IRoamArea _roamArea;

        private IMobPerUpdateData _perUpdateData;
        private MobConfig _config;

        private IMoveAbility _moveAbility;
        private IRotateAbility _rotateAbility;

        public bool IsAvailableToGet { get; private set; }

        [Inject]
        public void AddDependencies(IMobStateMachine stateMachine, IMobPerUpdateData perUpdateData,
            MobConfig config, IMoveAbility moveAbility, IRotateAbility rotateAbility, IRoamArea roamArea)
        {
            _perUpdateData = perUpdateData;
            _config = config;
            _roamArea = roamArea;

            _mobStateMachine = stateMachine;

            _moveAbility = moveAbility;
            _rotateAbility = rotateAbility;

            moveAbility.Init(config.Acceleration, config.MaxAcceleration);
            rotateAbility.Init(config.RotationSpeed, config.RotationDamper);
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
        }

        public void OnGet()
        {
            IsAvailableToGet = false;
        }

        public void OnReturn()
        {
            _spawnPoint.RemoveMob();
            IsAvailableToGet = true;
        }

        public void BindSpawnPoint(MobsSpawnPoint spawnPoint)
        {
            _spawnPoint = spawnPoint;
            _roamArea.Init(spawnPoint.RoamZone, _config.RoamingMinPathLength);
        }
    }
}
