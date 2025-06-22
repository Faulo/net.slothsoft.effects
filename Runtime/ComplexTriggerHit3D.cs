using Slothsoft.UnityExtensions;
using UnityEngine;
namespace Slothsoft.Events {
    public readonly struct ComplexTriggerHit3D : ITriggerHit {
        public Vector2 pointSum { get; }
        public int pointCount { get; }
        public ComplexTriggerHit3D(Collider colliderA, Collider colliderB) {
            pointCount = 0;
            pointSum = Vector2.zero;
            if (!(colliderA is MeshCollider meshA && !meshA.convex)) {
                pointCount++;
                pointSum += colliderA.ClosestPoint(colliderB.transform.position).SwizzleXY();
            }

            if (!(colliderB is MeshCollider meshB && !meshB.convex)) {
                pointCount++;
                pointSum += colliderB.ClosestPoint(colliderA.transform.position).SwizzleXY();
            }
        }
    }
}
