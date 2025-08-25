using System.Collections.Generic;
using System.Linq;
using CrossProject.Core;
using CrossProject.Core.SpawnPoints;
using CrossProject.Extensions;
using UnityEngine;
using VContainer;

namespace L2Farm.Features.QuestIndication
{
    public class QuestIndication : MonoBehaviour
    {
        private SpawnPointService _spawnPointService;

        [SerializeField] private float radius;
        [SerializeField] private Indicator prefab;
        [SerializeField] private IndicationTypeSetConfig indicationTypeSetConfig;

        private readonly Dictionary<SpawnPointId, (Vector3 target, Indicator instance)> _activeIndicators = new();

        private void Start()
        {
            ManualPrefabInjector.Instance.Inject(this);
        }

        [Inject]
        private void Construct(SpawnPointService spawnPointService)
        {
            _spawnPointService = spawnPointService;
        }

        public void AddTarget(SpawnPointId spawnPointId, IndicationTypeId indicationTypeId)
        {
            if (_activeIndicators.ContainsKey(spawnPointId))
            {
                Debug.LogError($"[{nameof(QuestIndication)}] : {spawnPointId} is busy by some other indicator!");
                return;
            }

            var instance = Instantiate(prefab, Vector3.zero, Quaternion.identity, transform);
            var config = indicationTypeSetConfig.items.FirstOrDefault(x => x.id == indicationTypeId);
            if (config != null)
                instance.SetUp(config.icon, config.color);
            instance.SetRadius(radius);
            _activeIndicators.Add(spawnPointId, (_spawnPointService.GetPosition(spawnPointId), instance));
        }

        public void RemoveTarget(SpawnPointId spawnPointId)
        {
            if (!_activeIndicators.TryGetValue(spawnPointId, out var pair))
            {
                Debug.LogError($"[{nameof(QuestIndication)}] : No such indicator");
                return;
            }

            Destroy(pair.instance.gameObject);
            _activeIndicators.Remove(spawnPointId);
        }

        private void LateUpdate()
        {
            foreach (var spawnPointId in _activeIndicators.Keys)
            {
                var pair = _activeIndicators[spawnPointId];
                var instancePos = pair.instance.transform.position.WithY(0);
                var targetPos = pair.target.WithY(0);
                pair.instance.transform.forward = targetPos - instancePos;
            }
        }
    }
}
