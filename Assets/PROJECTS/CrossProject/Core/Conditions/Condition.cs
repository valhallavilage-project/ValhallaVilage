using System;

namespace CrossProject.Core.Conditions
{
    public abstract class Condition<TConditionConfig> : ICondition where TConditionConfig : IConditionConfig
    {
        protected readonly ConditionService conditionService;
        protected TConditionConfig config;

        public Type ConfigType => typeof(TConditionConfig);

        public Condition(ConditionService conditionService)
        {
            this.conditionService = conditionService;
        }

        public void SetConfig(IConditionConfig conditionConfig)
        {
            if (conditionConfig is not TConditionConfig tconfig)
                throw new Exception($"Wrong condition config type! Expected : {typeof(TConditionConfig)}, but got {conditionConfig.GetType().Name}");

            config = tconfig;
        }

        public abstract bool Check();
    }
}
