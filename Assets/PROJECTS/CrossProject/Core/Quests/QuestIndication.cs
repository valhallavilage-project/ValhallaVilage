using System.Collections.Generic;
using CrossProject.Core.SpawnPoints;
using UnityEngine;
using VContainer;

namespace CrossProject.Core.Quests
{
    public class QuestIndication : MonoBehaviour
    {
        [SerializeField] private Transform root;
        [SerializeField] private GameObject indicationPrefab;

        private SpawnPointService _spawnPointService;

        private Dictionary<SpawnPointId, (Vector3 position, Transform indication)> _indicators = new();

        [Inject]
        private void Construct(SpawnPointService spawnPointService)
        {
            _spawnPointService = spawnPointService;
        }

        public void RegisterIndication(SpawnPointId spawnPointId)
        {
            _indicators.Add(spawnPointId, (_spawnPointService.GetPosition(spawnPointId), Instantiate(indicationPrefab, root).transform));
        }

        public void RemoveIndication(SpawnPointId spawnPointId)
        {
            if (_indicators.TryGetValue(spawnPointId, out var pair))
            {
                Destroy(pair.indication.gameObject);
                _indicators.Remove(spawnPointId);
            }
        }

        private void FixedUpdate()
        {
            foreach (var spawnPointId in _indicators.Keys)
            {
                _indicators[spawnPointId].indication.LookAt(_indicators[spawnPointId].position);
                var angles = _indicators[spawnPointId].indication.rotation.eulerAngles;
                angles.x = angles.z = 0;
                _indicators[spawnPointId].indication.rotation = Quaternion.Euler(angles);
            }
        }
    }
}
