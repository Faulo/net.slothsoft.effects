using UnityEngine;

namespace Slothsoft.Events {
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
