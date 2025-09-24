using CrossProject.Core.Actions;
using CrossProject.Core.Characters;
using CrossProject.Core.Quests;
using L2Farm.Scripts.Conditions;
using Sirenix.OdinInspector;
using UnityEngine;

namespace L2Farm.Features.SimpleMonolog
{
    [System.Serializable, HideReferenceObjectPicker]
    public class ShowMonologActionConfig : IActionConfig
    {
        public CharacterId speaker;
        public string message;
        public QuestId questId;
    }
}
