using System;
using Cysharp.Threading.Tasks;

namespace CrossProject.Core.Actions
{
    public interface IAction
    {
        Type ConfigType { get; }
        void SetConfig(IActionConfig actionConfig);
        UniTask Execute();
    }
}
