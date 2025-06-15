using UnityEngine;

namespace CrossProject.Core.SimpleMovement
{
    public interface IJoystickValueProvider
    {
        Vector2 NormalizedValue { get; }
        Vector3 NormalizedValueProjectOnPlane { get; }
    }
}