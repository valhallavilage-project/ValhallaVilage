using UnityEngine;
using UnityEngine.AI;

namespace CrossProject.Core
{
    public interface IRoamBoxArea : IRoamArea
    {
    }

    public class RoamBoxArea : IRoamBoxArea
    {
        private const int MaxNavMeshSampleAttempts = 10;
        private const float NavMeshSampleRadius = 5f;

        private BoxCollider _area;

        public float MinRoamPathLength { get; private set; }

        public void Init(Collider area, float minRoamPathLength)
        {
            _area = (BoxCollider)area;
            MinRoamPathLength = minRoamPathLength;
        }

        public bool IsInside(Vector3 position)
        {
            return _area.bounds.Contains(position);
        }

        public Vector3 GetPointInside()
        {
            var bounds = _area.bounds;

            for (var i = 0; i < MaxNavMeshSampleAttempts; i++)
            {
                var point = new Vector3(
                    Random.Range(bounds.min.x, bounds.max.x),
                    bounds.center.y,
                    Random.Range(bounds.min.z, bounds.max.z)
                );

                // Check if point is inside collider
                if (point != _area.ClosestPoint(point))
                {
                    continue;
                }

                // Sample NavMesh to get valid walkable position
                if (NavMesh.SamplePosition(point, out var hit, NavMeshSampleRadius, NavMesh.AllAreas))
                {
                    return hit.position;
                }
            }

            // Fallback: return center of bounds sampled on NavMesh
            if (NavMesh.SamplePosition(bounds.center, out var centerHit, NavMeshSampleRadius * 2, NavMesh.AllAreas))
            {
                return centerHit.position;
            }

            return bounds.center;
        }
    }
}