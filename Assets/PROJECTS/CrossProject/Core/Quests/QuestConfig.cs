using System.Collections.Generic;
using CrossProject.Core.Actions;
using CrossProject.Core.Conditions;
using CrossProject.Core.SpawnPoints;
using Sirenix.OdinInspector;
using UnityEngine;

namespace CrossProject.Core.Quests
{
    [System.Serializable, HideReferenceObjectPicker]
    public class QuestConfig
    {
        [FoldoutGroup("$id")] public string id;
        [FoldoutGroup("$id")] public SpawnPointId targetSpawnPoint;

        [FoldoutGroup("$id/Launch"), SerializeReference] public IConditionConfig launchCondition;
        [FoldoutGroup("$id/Launch"), SerializeReference] public List<IActionConfig> launchActions = new();

        [FoldoutGroup("$id"), SerializeReference] public List<QuestStepConfig> steps = new();

        [FoldoutGroup("$id/Win"), SerializeReference] public IConditionConfig winCondition;
        [FoldoutGroup("$id/Win"), SerializeReference] public List<IActionConfig> winActions = new();

        [FoldoutGroup("$id/Lose"), SerializeReference] public IConditionConfig loseCondition;
        [FoldoutGroup("$id/Lose"), SerializeReference] public List<IActionConfig> loseActions = new();
    }
}
