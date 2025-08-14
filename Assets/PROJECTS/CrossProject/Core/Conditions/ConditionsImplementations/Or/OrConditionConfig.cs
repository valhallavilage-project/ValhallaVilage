using System.Collections.Generic;

namespace CrossProject.Core.Conditions.ConditionsImplementations
{
    public class OrConditionConfig : IConditionConfig
    {
        public List<IConditionConfig> conditions;
    }
}
