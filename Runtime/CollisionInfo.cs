using UnityEngine;

namespace Slothsoft.Effects {
    public readonly struct CollisionInfo {
        public static readonly CollisionInfo empty = new();

        readonly CollisionIdentifier id;

        public readonly GameObject gameObject => id.gameObject;
        public readonly Vector2Int normal => id.normal;
        public readonly Quaternion normalRotation => AngleUtils.DirectionalRotation(id.normal);
        public readonly CollisionMaterial material => id.material;
        public readonly Vector3 point;
        public readonly float impulse;

        public CollisionInfo(CollisionIdentifier id, Vector3 point, float impulse) {
            this.id = id;
            this.point = point;
            this.impulse = impulse;
        }

        public CollisionInfo(GameObject gameObject) : this(new(gameObject), gameObject.transform.position, 0) {
        }

        /// <summary>
        /// Constructs a <see cref="CollisionInfo"/> from a 3D collsiion.
        /// </summary>
        /// <param name="collision"></param>
        /// <returns></returns>
        public static CollisionInfo FromCollision(Collision collision) => new(new(collision.gameObject), Vector3.zero, collision.impulse.magnitude);

        /// <summary>
        /// Constructs a <see cref="CollisionInfo"/> from a <see cref="GameObject"/> hitting a <see cref="Collider"/>.
        /// </summary>
        /// <param name="collision"></param>
        /// <returns></returns>
        public static CollisionInfo FromTrigger(GameObject self, Collider other) => new(new(self), other.ClosestPoint(self.transform.position), 0);

        public static CollisionInfo FromPoint(GameObject gameObject, Vector3 position) => new(new(gameObject), position, 0);

        public void SendCollisionMessage(string name) {
#if UNITY_EDITOR
            if (!id.gameObject) {
                Debug.LogError($"Can't send {nameof(CollisionInfo)} about destroyed {nameof(GameObject)} '{this}'!");
            }
#endif
            id.gameObject.SendMessage(name, this, SendMessageOptions.DontRequireReceiver);
        }

        public void BroadcastCollisionMessage(string name) {
#if UNITY_EDITOR
            if (!id.gameObject) {
                Debug.LogError($"Can't broadcast {nameof(CollisionInfo)} about destroyed {nameof(GameObject)} '{this}'!");
            }
#endif
            id.gameObject.BroadcastMessage(name, this, SendMessageOptions.DontRequireReceiver);
        }

        public override string ToString() => $"Hit {gameObject} at {point} with force {impulse}";
    }
}
