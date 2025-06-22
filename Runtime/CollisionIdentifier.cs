using System;
using UnityEngine;
using UnityEngine.Assertions;

namespace Slothsoft.Events {
    public readonly struct CollisionIdentifier : IEquatable<CollisionIdentifier> {
        public readonly GameObject gameObject;
        public readonly CollisionMaterial material;
        public readonly Vector2Int normal;
        public readonly bool isValid;

        public CollisionIdentifier(GameObject gameObject, CollisionMaterial material, Vector2Int normal = default) {
            Assert.IsTrue(gameObject);
            Assert.IsNotNull(material);

            this.gameObject = gameObject;
            this.material = material;
            this.normal = normal;

            isValid = true;
        }

        internal CollisionIdentifier(GameObject gameObject) {
            Assert.IsTrue(gameObject);

            this.gameObject = gameObject;
            material = default;
            normal = Vector2Int.zero;

            isValid = false;
        }

        public bool Equals(CollisionIdentifier other)
            => other.gameObject == gameObject
            && other.material == material
            && other.normal == normal;

        public override bool Equals(object obj)
            => obj is CollisionIdentifier other && Equals(other);

        public override int GetHashCode() => isValid
            ? gameObject.GetInstanceID()
            : 0;

        public static bool operator ==(CollisionIdentifier left, CollisionIdentifier right) => left.Equals(right);
        public static bool operator !=(CollisionIdentifier left, CollisionIdentifier right) => !left.Equals(right);
    }
}
