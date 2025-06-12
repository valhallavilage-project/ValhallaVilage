using System.Collections.Generic;
using Cysharp.Threading.Tasks;

namespace CrossProject.Core
{
    public abstract class AbstractGameLoader
    {
        public abstract List<UniTask> PrepareGameLoad();
    }
}