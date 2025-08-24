using CrossProject.Core.Conditions;
using CrossProject.Core.Quests;

namespace L2Farm.Scripts.Conditions
{
    [System.Serializable]
    public class NotEnoughResourcesConditionConfig : IConditionConfig
    {
        public QuestId questId;
        public int stepIndexWithResourceCondition;
    }
}
