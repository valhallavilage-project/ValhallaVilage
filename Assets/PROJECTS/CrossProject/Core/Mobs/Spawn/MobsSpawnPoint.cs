using System;
using Cysharp.Threading.Tasks;
using Cysharp.Threading.Tasks.Linq;
using Cysharp.Threading.Tasks.Triggers;
using UnityEngine;
using Random = UnityEngine.Random;

namespace CrossProject.Core
{
    [RequireComponent(typeof(MobsPool), typeof(SphereCollider))]
    public class MobsSpawnPoint : MonoBehaviour
    {
        [Header("Spawner Settings")]
        [SerializeField] private float _spawnRadius = 5f;
        [SerializeField] private float _spawnInterval = 2f;
        [SerializeField] private int _maxMobs = 10;
        [SerializeField] private SphereCollider _roamZone;
        [SerializeField] private SphereCollider _agroZone;

        private MobsPool _pool;
        private int _spawnedMobs;
        
        public SphereCollider AgroZone => _agroZone;
        public SphereCollider RoamZone => _roamZone;

        private void Awake()
        {
            _pool = GetComponent<MobsPool>();
            _roamZone = GetComponent<SphereCollider>();
        }

        private void Start()
        {
            SpawnLoop().Forget();
        }

        private async UniTask SpawnLoop()
        {
            while (true)
            {
                if (_spawnedMobs >= _maxMobs)
                {
                    await UniTask.Yield();
                    continue;
                }
                
                await UniTask.Delay(TimeSpan.FromSeconds(_spawnInterval), cancellationToken: gameObject.GetCancellationTokenOnDestroy());

                if (_spawnedMobs >= _maxMobs)
                {
                    await UniTask.WaitUntil(() => _spawnedMobs < _maxMobs, cancellationToken: gameObject.GetCancellationTokenOnDestroy());
                }

                SpawnMob();
            }
        }

        private void SpawnMob()
        {
            var randomPos = transform.position + Random.insideUnitSphere*_spawnRadius;
            randomPos.y = transform.position.y;

            var mobComponent = _pool.Get();

            mobComponent.transform.position = randomPos;
            mobComponent.transform.parent = transform;

            mobComponent.BindSpawnPoint(this);

            _spawnedMobs++;
        }

        public void RemoveMob()
        {
            _spawnedMobs--;
        }

        private void OnValidate()
        {
            if (_roamZone == null)
            {
                _roamZone = GetComponent<SphereCollider>();
            }

            _roamZone.radius = _spawnRadius;
        }
    }
}
