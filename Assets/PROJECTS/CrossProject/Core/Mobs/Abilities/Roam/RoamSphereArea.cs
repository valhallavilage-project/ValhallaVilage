using UnityEngine;
using UnityEngine.AI;

namespace CrossProject.Core
{
    public class RoamSphereArea : IRoamArea
    {
        private const int MaxNavMeshSampleAttempts = 10;
        private const float NavMeshSampleRadius = 5f;

        private SphereCollider _area;

        public float MinRoamPathLength { get; private set; }

        public void Init(Collider area, float minRoamPathLength)
        {
            _area = (SphereCollider)area;
            MinRoamPathLength = minRoamPathLength;
        }

        public bool IsInside(Vector3 position)
        {
            var center = _area.transform.position + _area.center;
            var radius = _area.radius * _area.transform.lossyScale.x;

            var distance = Vector3.Distance(position, center);

            return distance <= radius;
        }

        public Vector3 GetPointInside()
        {
            var center = _area.transform.position;
            var radius = _area.radius * _area.transform.lossyScale.x;

            for (var i = 0; i < MaxNavMeshSampleAttempts; i++)
            {
                var randomPoint = Random.insideUnitCircle * radius;
                var point = center + new Vector3(randomPoint.x, 0, randomPoint.y);

                // Sample NavMesh to get valid walkable position
                if (NavMesh.SamplePosition(point, out var hit, NavMeshSampleRadius, NavMesh.AllAreas))
                {
                    return hit.position;
                }
            }

            // Fallback: return center sampled on NavMesh
            if (NavMesh.SamplePosition(center, out var centerHit, NavMeshSampleRadius * 2, NavMesh.AllAreas))
            {
                return centerHit.position;
            }

            return center;
        }
    }
}
