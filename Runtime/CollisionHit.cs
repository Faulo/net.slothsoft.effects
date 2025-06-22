using UnityEngine;
namespace Slothsoft.Events {
    readonly struct CollisionHit : ICollisionHit {
        public Vector2 point { get; }

        public CollisionHit(Vector2 point) {
            this.point = point;
        }
    }
}
