using System.Collections.Generic;
using CrossProject.Core.Actions;
using CrossProject.Core.Conditions;
using Sirenix.OdinInspector;
using UnityEngine;

namespace CrossProject.Core.Quests
{
    [System.Serializable]
    public class QuestStepConfig
    {
        [SerializeReference] public IActionConfig stepAction;

        [SerializeReference, FoldoutGroup("Win")] public IConditionConfig winCondition;
        [SerializeReference, FoldoutGroup("Win")] public List<IActionConfig> winActions = new();

        [SerializeReference, FoldoutGroup("Lose")] public IConditionConfig loseCondition;
        [SerializeReference, FoldoutGroup("Lose")] public List<IActionConfig> loseActions = new();
    }
}
