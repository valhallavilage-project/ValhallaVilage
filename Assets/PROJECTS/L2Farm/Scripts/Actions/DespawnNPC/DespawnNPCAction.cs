using CrossProject.Core.Actions;
using L2Farm.Features.NPC;

namespace L2Farm.Scripts.Actions
{
    public class DespawnNPCAction : Action<DespawnNPCActionConfig>
    {
        private readonly NPCService _npcService;

        public DespawnNPCAction(NPCService npcService)
        {
            _npcService = npcService;
        }

        public override void Execute()
        {
            _npcService.DespawnNPC(config.npcId);
        }
    }
}
