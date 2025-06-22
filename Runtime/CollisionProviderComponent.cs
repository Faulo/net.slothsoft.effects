using UnityEngine;

namespace Slothsoft.Effects {
    public sealed class CollisionProviderComponent : MonoBehaviour, ICollisionProvider {
        public CollisionInfo collision { get; set; }
    }
}
