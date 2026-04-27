using System.Collections.Generic;
using System.Linq;
using CrossProject.Core;
using CrossProject.Core.Actions;
using CrossProject.Core.Quests;
using CrossProject.Core.SpawnPoints;
using Cysharp.Threading.Tasks;
using UnityEngine;
using VContainer.Unity;

namespace L2Farm.Features.NPC
{
    public class NPCService : IInitializable
    {
        private readonly AddressablesManager _addressablesManager;
        private readonly SpawnPointService _spawnPointService;
        private readonly QuestService _questService;

        private NPCSetConfig _npcSetConfig;

        private Dictionary<NPCId, (SpawnPointId spawnPointId, NPCInteractiveObject instance)> _npcs = new ();
        private ActionService _actionService;

        public bool IsInitialized { get; private set; }

        public NPCService(
            AddressablesManager addressablesManager,
            SpawnPointService spawnPointService,
            QuestService questService,
            ActionService actionService)
        {
            _actionService = actionService;
            _addressablesManager = addressablesManager;
            _spawnPointService = spawnPointService;
            _questService = questService;
        }

        public async UniTask Initialize()
        {
            _npcSetConfig = await _addressablesManager.LoadAssetAsync<NPCSetConfig>();
            await UniTask.WaitUntil(() => _spawnPointService.IsInitialized && _questService.IsInitialized);
            foreach (var npcConfig in _npcSetConfig.items)
            {
                if (_npcs.ContainsKey(npcConfig.id))
                    continue;
                SpawnNPC(npcConfig.id, npcConfig.defaultSpawnPoint, null);
            }
            IsInitialized = true;
        }

        public void SpawnNPC(NPCId id, SpawnPointId spawnPointId, QuestId questId)
        {
            var config = _npcSetConfig.items.FirstOrDefault(x => x.id == id);
            if (config == null)
            {
                Debug.LogError($"[{nameof(NPCService)}] : there is no NPC with id : {id}");
                return;
            }

            Debug.Log($"[DailyDebug][NPCService] SpawnNPC id={id} spawnPointId={spawnPointId} questId={questId}");

            if (!_npcs.TryGetValue(id, out var pair) || pair.spawnPointId != spawnPointId)
            {
                if (pair.instance != null)
                    DespawnNPC(id);
                var spawnPointPosition = _spawnPointService.GetPosition(spawnPointId);
                var spawnPointRotation = _spawnPointService.GetEulerAngles(spawnPointId);
                var instance = Object.Instantiate(config.prefab, spawnPointPosition, Quaternion.Euler(spawnPointRotation));
                var component = instance.GetComponent<NPCInteractiveObject>()
                                ?? instance.GetComponentInChildren<NPCInteractiveObject>(true);
                if (component == null)
                {
                    Debug.LogError($"[{nameof(NPCService)}] : NPC prefab for {id} has no NPCInteractiveObject component.");
                    Object.Destroy(instance);
                    return;
                }
                Debug.Log($"[DailyDebug][NPCService] Instantiated {id}, NPCInteractiveObject found on {(component.gameObject == instance ? "root" : "child")}");
                component.SetQuest(questId, _questService, _actionService);
                _npcs[id] = (spawnPointId, component);
                return;
            }

            if (pair.instance.CurrentQuestId != questId)
            {
                Debug.Log($"[DailyDebug][NPCService] Updating quest for existing {id}: {pair.instance.CurrentQuestId} -> {questId}");
                pair.instance.SetQuest(questId, _questService, _actionService);
            }
        }

        public void DespawnNPC(NPCId id)
        {
            var npc = _npcs[id].instance;
            Object.Destroy(npc.transform.root.gameObject);
            _npcs.Remove(id);
            Debug.Log($"[{nameof(NPCService)}] : Despawned {id}");
        }
    }
}
