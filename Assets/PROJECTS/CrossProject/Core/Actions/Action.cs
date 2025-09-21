using System;
using Cysharp.Threading.Tasks;

namespace CrossProject.Core.Actions
{
    public abstract class Action<TActionConfig> : IAction where TActionConfig : IActionConfig
    {
        protected TActionConfig config;

        public Type ConfigType => typeof(TActionConfig);

        public void SetConfig(IActionConfig actionConfig)
        {
            if (actionConfig is not TActionConfig tconfig)
                throw new Exception($"Wrong condition config type! Expected : {typeof(TActionConfig).Name}, but got {actionConfig.GetType().Name}");

            config = tconfig;
        }

        public abstract UniTask Execute();
    }
}
