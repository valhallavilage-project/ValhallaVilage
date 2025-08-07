using System;

namespace CrossProject.Core.Conditions
{
    public interface ICondition
    {
        Type ConfigType { get; }

        void SetConfig(IConditionConfig conditionConfig);

        bool Check();
    }
}
