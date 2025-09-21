using CrossProject.Core.Actions;
using Cysharp.Threading.Tasks;
using L2Farm.Features.NPC;

namespace L2Farm.Scripts.Actions
{
    public class SpawnNPCAction : Action<SpawnNPCActionConfig>
    {
        private readonly NPCService _npcService;

        public SpawnNPCAction(NPCService npcService)
        {
            _npcService = npcService;
        }

        public override async UniTask Execute()
        {
            _npcService.SpawnNPC(config.npcId, config.spawnPointId, config.questId);
        }
    }
}
