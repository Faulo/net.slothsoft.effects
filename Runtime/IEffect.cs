using UnityEngine;

namespace Slothsoft.Events {
    public interface IEffect {
        void Invoke();
        void Invoke(GameObject context);
        void Invoke(CollisionInfo collision);
    }
}
