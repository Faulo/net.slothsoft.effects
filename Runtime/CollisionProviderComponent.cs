using UnityEngine;

namespace Slothsoft.Events {
    public sealed class CollisionProviderComponent : MonoBehaviour, ICollisionProvider {
        public CollisionInfo collision { get; set; }
    }
}
