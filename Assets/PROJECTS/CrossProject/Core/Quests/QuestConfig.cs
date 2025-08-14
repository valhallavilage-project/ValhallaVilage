using System.Collections.Generic;
using CrossProject.Core.Actions;
using CrossProject.Core.Conditions;
using UnityEngine;

namespace CrossProject.Core.Quests
{
    [System.Serializable]
    public class QuestConfig
    {
        public string id;

        public IConditionConfig launchCondition;
        public IConditionConfig winCondition;
        public IConditionConfig loseCondition;

        public List<IActionConfig> launchActions = new();
        public List<IActionConfig> winActions = new();
        public List<IActionConfig> loseActions = new();

        [SerializeReference]
        public List<QuestStepConfig> steps = new();
    }
}
