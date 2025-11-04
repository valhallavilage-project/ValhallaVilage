using System;
using Cysharp.Threading.Tasks;
using UnityEngine;
using VContainer;
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
        [SerializeField] private bool _isMobsFiniteAmount;
        [SerializeField] private SphereCollider _roamZone;
        [SerializeField] private SphereCollider _agroZone;
        [SerializeField] private string _id;

        private MobsPool _pool;
        private int _spawnedMobs;
        private IMobsSpawnsService _mobsSpawnsService;

        public SphereCollider AgroZone => _agroZone;
        public SphereCollider RoamZone => _roamZone;

        [Inject]
        private void AddDependencies(IMobsSpawnsService mobsSpawnsService)
        {
            _mobsSpawnsService = mobsSpawnsService;

            if (_isMobsFiniteAmount)
            {
                _spawnedMobs = _mobsSpawnsService.GetMobsSpawned(_id);
            }
        }

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
            var randomPos = transform.position + Random.insideUnitSphere * _spawnRadius;
            randomPos.y = transform.position.y;

            var mobComponent = _pool.Get();

            mobComponent.transform.position = randomPos;
            mobComponent.transform.parent = transform;

            mobComponent.BindSpawnPoint(this);

            _spawnedMobs++;

            if (_isMobsFiniteAmount)
            {
                _mobsSpawnsService.IncrementSpawnedMob(_id);
            }
        }

        public void RemoveMob()
        {
            if (!_isMobsFiniteAmount)
            {
                _spawnedMobs--;
            }
        }

        private void OnValidate()
        {
            if (_roamZone == null)
            {
                _roamZone = GetComponent<SphereCollider>();
            }

            _roamZone.radius = _spawnRadius;

#if UNITY_EDITOR
            if (string.IsNullOrEmpty(_id) && !Application.isPlaying && !UnityEditor.PrefabUtility.IsPartOfPrefabAsset(this) &&
                UnityEditor.SceneManagement.PrefabStageUtility.GetCurrentPrefabStage() == null)
            {
                _id = Guid.NewGuid().ToString();
            }
  #endif
        }
    }
}
