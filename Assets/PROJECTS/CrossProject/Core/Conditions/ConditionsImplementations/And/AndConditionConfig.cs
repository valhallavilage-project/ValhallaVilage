using System.Collections.Generic;
using Sirenix.OdinInspector;

namespace CrossProject.Core.Conditions.ConditionsImplementations
{
    [System.Serializable]
    public class AndConditionConfig : IConditionConfig
    {
        public List<IConditionConfig> conditions;
    }
}
