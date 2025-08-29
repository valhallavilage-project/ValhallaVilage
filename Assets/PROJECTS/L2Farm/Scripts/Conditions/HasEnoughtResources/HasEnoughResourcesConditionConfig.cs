using System.Collections.Generic;
using CrossProject.Core.Conditions;
using CrossProject.Core.InGameResources;
using Sirenix.OdinInspector;

namespace L2Farm.Scripts.Conditions
{
    [System.Serializable, HideReferenceObjectPicker]
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
