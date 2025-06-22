using MyBox;
using Slothsoft.UnityExtensions;
using UnityEngine;

namespace Slothsoft.Events {
    public sealed class CollisionMaterial {
        public bool IsMaterial(PhysicsMaterial2D material)
            => material2D == material;
        public bool IsMaterial(PhysicsMaterial material)
            => material3D == material;

        public static implicit operator CollisionMaterial(PhysicsMaterial material)
            => new() { material3D = material };
        public static implicit operator CollisionMaterial(PhysicsMaterial2D material)
            => new() { material2D = material };

        public void ApplyTo(Collider2D collider) {
            collider.sharedMaterial = material2D;
        }
        public void ApplyTo(Rigidbody2D rigidbody) {
            rigidbody.sharedMaterial = material2D;
        }
        public void ApplyTo(Collider collider) {
            collider.sharedMaterial = material3D;
        }

        [Header("Physics")]
        [SerializeField, Range(0, 10)]
        public float dynamicFriction = 0;
        [SerializeField, Range(0, 10)]
        public float staticFriction = 0;
        [SerializeField, Range(0, 10)]
        public float bounciness = 0;

        [Header("Material Handles")]
        [SerializeField, ReadOnly, Expandable]
        internal PhysicsMaterial2D material2D = default;
        [SerializeField, ReadOnly, Expandable]
        internal PhysicsMaterial material3D = default;
    }
}
