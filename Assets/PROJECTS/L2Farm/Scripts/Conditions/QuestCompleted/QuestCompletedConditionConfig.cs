using CrossProject.Core.Conditions;
using CrossProject.Core.Quests;
using Sirenix.OdinInspector;

namespace L2Farm.Scripts.Conditions.QuestCompleted
{
    [HideReferenceObjectPicker]
    public class QuestCompletedConditionConfig : IConditionConfig
    {
        public QuestId questId;
    }
}
