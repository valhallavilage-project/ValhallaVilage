using UnityEngine;

namespace CrossProject.Core.SimpleMovement
{
    public interface IJoystickValueProvider
    {
        Vector2 NormalizedVector2 { get; }
        Vector3 NormalizedVector3 { get; }
    }
}