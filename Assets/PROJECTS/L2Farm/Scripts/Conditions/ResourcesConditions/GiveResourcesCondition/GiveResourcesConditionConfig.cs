using System.Collections.Generic;
using CrossProject.Core.Conditions;
using L2Farm.Scripts.Conditions;
using UnityEngine;

namespace L2Farm.Scripts
{
    [System.Serializable]
    public class GiveResourcesConditionConfig : IConditionConfig
    {
        [SerializeField] private List<ResourceCondition> _resourceConditions = new();

        public List<ResourceCondition> ResourceConditions => _resourceConditions;
    }
}
