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

            if (!_npcs.TryGetValue(id, out var pair) || pair.spawnPointId != spawnPointId)
            {
                if (pair.instance != null)
                    DespawnNPC(id);
                var spawnPointPosition = _spawnPointService.GetPosition(spawnPointId);
                var spawnPointRotation = _spawnPointService.GetEulerAngles(spawnPointId);
                var instance = Object.Instantiate(config.prefab, spawnPointPosition, Quaternion.Euler(spawnPointRotation));
                var component = instance.GetComponent<NPCInteractiveObject>();
                component.SetQuest(questId, _questService, _actionService);
                _npcs[id] = (spawnPointId, component);
                //Debug.Log($"[{nameof(NPCService)}] : Spawned NPC with id : {id} at {spawnPointId} with quest :{questId}.");
                return;
            }

            if (pair.instance.CurrentQuestId != questId)
            {
                pair.instance.SetQuest(questId, _questService, _actionService);
                Debug.Log($"[{nameof(NPCService)}] : Updated quest for {id} at {spawnPointId} with : {questId}.");
            }
        }

        public void DespawnNPC(NPCId id)
        {
            var npc = _npcs[id].instance;
            Object.Destroy(npc.gameObject);
            _npcs.Remove(id);
            Debug.Log($"[{nameof(NPCService)}] : Despawned {id}");
        }
    }
}
