using System;
using CrossProject.Core.Conditions;
using L2Farm.Features.ResourceProduction;
using UnityEngine;

namespace L2Farm.Scripts.Conditions
{
    [Serializable]
    public class ProductionCompletedConditionConfig : IConditionConfig
    {
        [SerializeField] private ProductionId _production;

        public ProductionId Production => _production;
    }
}
