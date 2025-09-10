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
        [FoldoutGroup("$id")] public bool proceedAfterLaunch;
        [FoldoutGroup("$id")] public SpawnPointId targetSpawnPoint;
        [FoldoutGroup("$id")] public IndicationTypeId questIndication;

        [FoldoutGroup("$id"), SerializeReference] public List<IActionConfig> launchActions = new();

        [FoldoutGroup("$id"), SerializeReference] public List<QuestStepConfig> steps = new();

        [FoldoutGroup("$id"), SerializeReference] public List<IActionConfig> winActions = new();

        [FoldoutGroup("$id"), SerializeReference] public List<IActionConfig> loseActions = new();
    }
}
