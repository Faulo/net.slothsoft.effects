using UnityEngine;
namespace Slothsoft.Effects {
    readonly struct CollisionHit : ICollisionHit {
        public Vector2 point { get; }

        public CollisionHit(Vector2 point) {
            this.point = point;
        }
    }
}
