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

        [Space(25)]
        [FoldoutGroup("$id"), SerializeReference] public IConditionConfig launchCondition;
        [FoldoutGroup("$id"), SerializeReference] public List<IActionConfig> launchActions = new();

        [Space(25)]
        [FoldoutGroup("$id"), SerializeReference] public List<QuestStepConfig> steps = new();

        [Space(25)]
        [FoldoutGroup("$id"), SerializeReference] public IConditionConfig winCondition;
        [FoldoutGroup("$id"), SerializeReference] public List<IActionConfig> winActions = new();

        [Space(25)]
        [FoldoutGroup("$id"), SerializeReference] public IConditionConfig loseCondition;
        [FoldoutGroup("$id"), SerializeReference] public List<IActionConfig> loseActions = new();

        [FoldoutGroup("$id")] public SpawnPointId targetSpawnPoint;
    }
}
