using System.Collections.Generic;
using CrossProject.Core.Actions;
using CrossProject.Core.Conditions;
using UnityEngine;

namespace CrossProject.Core.Quests
{
    [System.Serializable]
    public class QuestStepConfig
    {
        [SerializeReference] public IConditionConfig winCondition;
        [SerializeReference] public List<IActionConfig> winActions = new();

        [SerializeReference] public IActionConfig stepAction;

        [SerializeReference] public IConditionConfig loseCondition;
        [SerializeReference] public List<IActionConfig> loseActions = new();
    }
}
