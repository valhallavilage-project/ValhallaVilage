using CrossProject.Core.Actions;
using CrossProject.Core.Characters;
using CrossProject.Core.Quests;
using L2Farm.Scripts.Conditions;
using UnityEngine;

namespace L2Farm.Features.SimpleMonolog
{
    [System.Serializable]
    public class ShowMonologActionConfig : IActionConfig
    {
        public CharacterId speaker;
        public string message;
        public QuestId questId;
        public int stepIndexWithResourceCondition = -1;
    }
}
