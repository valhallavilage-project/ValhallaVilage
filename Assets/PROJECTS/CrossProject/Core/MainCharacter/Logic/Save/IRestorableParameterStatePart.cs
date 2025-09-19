using System;
using CrossProject.Core.SaveLoad;

namespace CrossProject.Core
{
    public interface IRestorableParameterStatePart : IGameStatePart
    {
        float Value { get; set; }
        DateTime LastRestoreTime { get; set; }
    }
}
