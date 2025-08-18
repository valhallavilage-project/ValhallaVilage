using CrossProject.Core.Actions;
using CrossProject.Core.Characters;
using CrossProject.Core.Quests;
using L2Farm.Scripts.Conditions;

namespace L2Farm.Features.SimpleMonolog
{
    public class ShowMonologActionConfig : IActionConfig
    {
        public CharacterId speaker;
        public string message;
        public QuestId questId;
        public HasEnoughResourcesConditionConfig resources;
        public IActionConfig onNextClickActionConfig;
    }
}
