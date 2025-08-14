using System.Collections.Generic;
using CrossProject.Core.Conditions;
using CrossProject.Core.InGameResources;

namespace L2Farm.Scripts.Conditions
{
    [System.Serializable]
    public class HasEnoughResourcesConditionConfig : IConditionConfig
    {
        public List<ResourceCondition> resourceConditions = new();
    }

    [System.Serializable]
    public class ResourceCondition
    {
        public ResourceId resourceId;
        public int neededQuantity;
    }
}
