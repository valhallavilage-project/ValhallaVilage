using System.Collections.Generic;
using System.Linq;
using CrossProject.Core;
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

        private NPCSetConfig _npcSetConfig;

        private Dictionary<SpawnPointId, NPCInteractiveObject> _npcInteractiveObjects = new ();

        public bool IsInitialized { get; private set; }

        public NPCService(
            AddressablesManager addressablesManager,
            SpawnPointService spawnPointService)
        {
            _addressablesManager = addressablesManager;
            _spawnPointService = spawnPointService;
        }

        public async UniTask Initialize()
        {
            _npcSetConfig = await _addressablesManager.LoadAssetAsync<NPCSetConfig>();
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

            if (_npcInteractiveObjects.ContainsKey(spawnPointId))
            {
                Debug.LogError($"[{nameof(NPCService)}] : spawnPoint {spawnPointId} is busy by {_npcInteractiveObjects[spawnPointId].gameObject.name}!\nCan't spawn {id} at the same place!");
                return;
            }

            var spawnPointPosition = _spawnPointService.GetPosition(spawnPointId);
            var spawnPointRotation = _spawnPointService.GetEulerAngles(spawnPointId);
            var instance = Object.Instantiate(config.prefab, spawnPointPosition, Quaternion.Euler(spawnPointRotation));
            var component = instance.GetComponent<NPCInteractiveObject>();
            component.SetQuest(questId);
            component.SetId(id);
        }

        public void DespawnNPC(NPCId id, SpawnPointId spawnPointId)
        {
            if (!_npcInteractiveObjects.TryGetValue(spawnPointId, out var npc))
            {
                Debug.LogError($"[{nameof(NPCService)}] : there is no npc spawned on {spawnPointId}!");
                return;
            }

            if (npc.Id != id)
            {
                Debug.LogError($"[{nameof(NPCService)}] : there is other npc spawned on {spawnPointId} - {npc.Id}!\n Can't despawn {id} from the same place!");
                return;
            }

            Object.Destroy(npc.gameObject);
            _npcInteractiveObjects.Remove(spawnPointId);
        }
    }
}
