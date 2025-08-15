using CrossProject.Core.Actions;
using CrossProject.Core.Characters;
using L2Farm.Scripts.Conditions;

namespace L2Farm.Scripts.Actions
{
    public class ShowMonologActionConfig : IActionConfig
    {
        public CharacterId speaker;
        public string message;
        public HasEnoughResourcesConditionConfig hasEnoughResourcesConditionConfig;
        public IActionConfig onNextClickActionConfig;
    }
}
