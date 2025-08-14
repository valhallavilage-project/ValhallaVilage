using System.Collections.Generic;

namespace CrossProject.Core.Conditions.ConditionsImplementations
{
    [System.Serializable]
    public class OrConditionConfig : IConditionConfig
    {
        public List<IConditionConfig> conditions;
    }
}
