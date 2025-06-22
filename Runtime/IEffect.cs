using UnityEngine;

namespace Slothsoft.Effects {
    public interface IEffect {
        void Invoke();
        void Invoke(GameObject context);
        void Invoke(CollisionInfo collision);
    }
}
