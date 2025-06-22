using UnityEngine;

namespace Slothsoft.Events {
    public interface ICursedAction {
        void Invoke();
        void Invoke(GameObject context);
        void Invoke(CollisionInfo collision);
    }
}
