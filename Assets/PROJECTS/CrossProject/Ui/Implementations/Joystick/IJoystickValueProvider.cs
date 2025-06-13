using UnityEngine;

namespace CrossProject.Ui.Implementations
{
    public interface IJoystickValueProvider
    {
        Vector2 NormalizedValue { get; }
    }
}