using System.Collections.Generic;
using CrossProject.Core.Actions;
using CrossProject.Core.Conditions;

namespace CrossProject.Core.Quests
{
    [System.Serializable]
    public abstract class QuestStepConfig
    {
        public IConditionConfig winCondition;
        public IConditionConfig loseCondition;

        public List<IActionConfig> winActions = new();
        public List<IActionConfig> loseActions = new();
    }
}
