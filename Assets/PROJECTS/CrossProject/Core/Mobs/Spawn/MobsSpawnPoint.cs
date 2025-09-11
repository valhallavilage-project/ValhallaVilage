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
        private bool _isPaused;
        private int _spawnedMobs;
        
        public SphereCollider AgroZone => _agroZone;
        public SphereCollider RoamZone => _roamZone;

        private void Awake()
        {
            _pool = GetComponent<MobsPool>();
            _roamZone = GetComponent<SphereCollider>();

            this.GetAsyncTriggerEnterTrigger().ForEachAsync(OnPlayerEnter, gameObject.GetCancellationTokenOnDestroy());
            this.GetAsyncTriggerExitTrigger().ForEachAsync(OnPlayerExit, gameObject.GetCancellationTokenOnDestroy());
        }

        private void Start()
        {
            SpawnLoop().Forget();
        }

        private async UniTask SpawnLoop()
        {
            while (true)
            {
                await UniTask.Delay(TimeSpan.FromSeconds(_spawnInterval), cancellationToken: gameObject.GetCancellationTokenOnDestroy());

                if (_spawnedMobs >= _maxMobs || _isPaused)
                {
                    await UniTask.WaitUntil(() => _spawnedMobs < _maxMobs && !_isPaused, cancellationToken: gameObject.GetCancellationTokenOnDestroy());
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
        
        private void OnPlayerEnter(Collider other)
        {
            if (other.CompareTag("Player"))
                _isPaused = true;
        }
        
        private void OnPlayerExit(Collider other)
        {
            if (other.CompareTag("Player"))
                _isPaused = false;
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
