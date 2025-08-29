using CrossProject.Core.Conditions;
using CrossProject.Core.Quests;
using Sirenix.OdinInspector;

namespace L2Farm.Scripts.Conditions
{
    [System.Serializable, HideReferenceObjectPicker]
    public class NotEnoughResourcesConditionConfig : IConditionConfig
    {
        public QuestId questId;
        public int stepIndexWithResourceCondition;
    }
}
