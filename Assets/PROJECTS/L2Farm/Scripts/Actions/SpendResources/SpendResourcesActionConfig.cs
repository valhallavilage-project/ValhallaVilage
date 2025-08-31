using System.Collections.Generic;
using CrossProject.Core.Actions;
using L2Farm.Scripts.Conditions;

namespace L2Farm.Scripts.Actions.SpendResources
{
    public class SpendResourcesActionConfig : IActionConfig
    {
        public List<ResourceCondition> resourceConditions = new();
    }
}
