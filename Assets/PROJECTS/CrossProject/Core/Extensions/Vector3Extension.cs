using UnityEngine;

namespace CrossProject.Core
{
    public static class Vector3Extension
    {
        public static bool IsDestinationReached(this Vector3 currentPosition, Vector3 destination, Vector3 startPosition,
            float epsilon = float.Epsilon, bool isIgnoreY = false)
        {
            if (isIgnoreY)
            {
                currentPosition = Vector3.ProjectOnPlane(currentPosition, Vector3.up);
                destination = Vector3.ProjectOnPlane(destination, Vector3.up);
                startPosition = Vector3.ProjectOnPlane(startPosition, Vector3.up);
            }

            var currentPositionToTargetPathLength = (destination - startPosition).magnitude;
            var currentPositionToStartPositionLength = (currentPosition - startPosition).magnitude;
            return currentPositionToTargetPathLength - currentPositionToStartPositionLength <= epsilon;
        }
    }
}
