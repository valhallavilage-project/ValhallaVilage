using CrossProject.Core.Actions;
using L2Farm.Features.NPC;
using Sirenix.OdinInspector;

namespace L2Farm.Scripts.Actions
{
    [HideReferenceObjectPicker]
    public class DespawnNPCActionConfig : IActionConfig
    {
        public NPCId npcId;
    }
}
