using System.Collections.Generic;

namespace CrossProject.Core.Conditions.ConditionsImplementations
{
    public class AndConditionConfig : IConditionConfig
    {
        public List<IConditionConfig> conditions;
    }
}
