using CrossProject.Core.Interactions;
using CrossProject.Core.Pooling;
using Cysharp.Threading.Tasks;
using UnityEngine;
using VContainer;

namespace CrossProject.Core
{
    public class Mob : AbstractInteractiveObject, IPoolElement
    {
        [SerializeField] private Rigidbody _rigidbody;

        private MobsSpawnPoint _spawnPoint;
        private IMobStateMachine _mobStateMachine;
        private IRoamArea _roamArea;

        private IMobPerUpdateData _perUpdateData;
        private MobConfig _config;

        public bool IsAvailableToGet { get; private set; }

        [Inject]
        public void AddDependencies(IMobStateMachine stateMachine, IMobPerUpdateData perUpdateData,
            MobConfig config, IMoveAbility moveAbility, IRotateAbility rotateAbility, IRoamArea roamArea)
        {
            _perUpdateData = perUpdateData;
            _config = config;
            _roamArea = roamArea;

            _mobStateMachine = stateMachine;

            moveAbility.Init(config.Acceleration, config.MaxAcceleration);
            rotateAbility.Init(config.RotationSpeed, config.RotationDamper);
        }

        private void FixedUpdate()
        {
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
            _roamArea.Init(spawnPoint.AgroZone, _config.RoamingMinPathLength);
        }

        protected override UniTask AfterInteraction()
        {
            return UniTask.CompletedTask;
        }
    }
}
