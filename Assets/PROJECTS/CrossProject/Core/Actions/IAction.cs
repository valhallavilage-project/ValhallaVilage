using System;

namespace CrossProject.Core.Actions
{
    public interface IAction
    {
        Type ConfigType { get; }
        void SetConfig(IActionConfig actionConfig);
        void Execute();
    }
}
