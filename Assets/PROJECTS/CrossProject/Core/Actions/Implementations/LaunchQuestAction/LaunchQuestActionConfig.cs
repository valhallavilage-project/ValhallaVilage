using CrossProject.Core.Quests;
using Sirenix.OdinInspector;

namespace CrossProject.Core.Actions.Implementations
{
    [HideReferenceObjectPicker]
    public class LaunchQuestActionConfig : IActionConfig
    {
        public QuestId questId;
    }
}
