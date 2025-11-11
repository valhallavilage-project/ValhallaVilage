using System.Collections.Generic;
using System.Linq;
using CrossProject.Core;
using CrossProject.Core.Quests;
using CrossProject.Core.SpawnPoints;
using UnityEngine;
using VContainer;

namespace L2Farm.Features.QuestIndication
{
    public class QuestIndication : MonoBehaviour
    {
        private SpawnPointService _spawnPointService;
        private QuestService _questService;

        [SerializeField] private float radius;
        [SerializeField] private float hideRadius = 10;
        [SerializeField] private Indicator prefab;
        [SerializeField] private IndicationTypeSetConfig indicationTypeSetConfig;

        private readonly Dictionary<QuestId, (Vector3 target, Indicator instance)> _activeIndicators = new();

        private void Start()
        {
            Injector.Instance.Inject(this);
        }

        [Inject]
        private void Construct(
            SpawnPointService spawnPointService,
            QuestService questService)
        {
            _spawnPointService = spawnPointService;
            _questService = questService;
            _questService.OnQuestLaunch += OnQuestLaunch;
            _questService.OnQuestWin += OnQuestComplete;
            _questService.OnQuestLose += OnQuestComplete;
        }

        private void OnQuestLaunch(QuestId questId)
        {
            var indication = _questService.GetConfigFor(questId).questIndication;
            if (!string.IsNullOrEmpty(indication))
                AddTarget(questId, indication);
        }

        private void OnQuestComplete(QuestId questId)
        {
            RemoveTarget(questId);
        }

        public void AddTarget(QuestId questId, IndicationTypeId indicationTypeId)
        {
            if (_activeIndicators.ContainsKey(questId))
            {
                //Debug.LogError($"[{nameof(QuestIndication)}] : {questId} is busy by some other indicator!");
                return;
            }

            var spawnPoint = _questService.GetConfigFor(questId).targetSpawnPoint;
            if (string.IsNullOrEmpty(spawnPoint))
                return;

            var instance = Instantiate(prefab, Vector3.zero, Quaternion.identity, transform);
            var config = indicationTypeSetConfig.items.FirstOrDefault(x => x.id == indicationTypeId);
            if (config != null)
                instance.SetUp(config.icon, config.color);
            instance.SetRadius(radius);
            _activeIndicators.Add(questId, (_spawnPointService.GetPosition(spawnPoint), instance));
            //Debug.Log($"[{nameof(QuestIndication)}] : Add for : {questId}");
        }

        public void RemoveTarget(QuestId questId)
        {
            if (!_activeIndicators.TryGetValue(questId, out var pair))
            {
                //Debug.LogError($"[{nameof(QuestIndication)}] : No such indicator");
                return;
            }

            Destroy(pair.instance.gameObject);
            _activeIndicators.Remove(questId);
            //Debug.Log($"[{nameof(QuestIndication)}] : Removed for : {questId}");
        }

        private void LateUpdate()
        {
            foreach (var spawnPointId in _activeIndicators.Keys)
            {
                var pair = _activeIndicators[spawnPointId];
                var instancePos = pair.instance.transform.position.WithY(0);
                var targetPos = pair.target.WithY(0);
                pair.instance.transform.localPosition = Vector3.zero;
                pair.instance.transform.forward = targetPos - instancePos;

                pair.instance.gameObject.SetActive(Vector3.Distance(transform.position, targetPos) > hideRadius);
            }
        }
    }
}
