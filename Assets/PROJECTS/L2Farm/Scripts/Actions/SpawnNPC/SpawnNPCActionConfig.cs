using CrossProject.Core.Actions;
using CrossProject.Core.Quests;
using CrossProject.Core.SpawnPoints;
using L2Farm.Features.NPC;
using Sirenix.OdinInspector;

namespace L2Farm.Scripts.Actions
{
    [HideReferenceObjectPicker]
    public class SpawnNPCActionConfig : IActionConfig
    {
        public NPCId npcId;
        public SpawnPointId spawnPointId;
        public QuestId questId;
    }
}
