using System.Collections.Generic;
using CrossProject.Core.Conditions;
using CrossProject.Core.InGameResources;
using UnityEngine;

namespace L2Farm.Scripts.Conditions
{
    [System.Serializable]
    public class HasEnoughResourcesConditionConfig : IConditionConfig
    {
        [SerializeField] private List<ResourceCondition> resourceConditions = new();

        public List<ResourceCondition> ResourceConditions => resourceConditions;
    }

    [System.Serializable]
    public class ResourceCondition
    {
        [SerializeField] private ResourceId resourceId;
        [SerializeField] private int neededQuantity;

        public ResourceId Id => resourceId;
        public int NeededAmount => neededQuantity;
    }
}
