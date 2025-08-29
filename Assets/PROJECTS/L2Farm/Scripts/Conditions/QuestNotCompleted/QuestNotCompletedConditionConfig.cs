using CrossProject.Core.Conditions;
using CrossProject.Core.Quests;
using Sirenix.OdinInspector;

namespace L2Farm.Scripts.Conditions.QuestNotCompleted
{
    [HideReferenceObjectPicker]
    public class QuestNotCompletedConditionConfig : IConditionConfig
    {
        public QuestId questId;
    }
}
