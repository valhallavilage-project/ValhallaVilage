using System.Collections.Generic;
using Sirenix.OdinInspector;

namespace CrossProject.Core.Conditions.ConditionsImplementations
{
    [System.Serializable]
    public class OrConditionConfig : IConditionConfig
    {
        public List<IConditionConfig> conditions;
    }
}
