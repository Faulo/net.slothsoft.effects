using System;
using Slothsoft.UnityExtensions;
using UnityEngine;
using UnityEngine.Events;

namespace Slothsoft.Events {
    [ImplementationFor(typeof(ICursedAction), nameof(GameObjectAction))]
    [Serializable]
    sealed class GameObjectAction : ICursedAction {
        [SerializeField]
        UnityEvent<GameObject> action = new();

        public void Invoke() => action.Invoke(default);
        public void Invoke(GameObject context) => action.Invoke(context);
        public void Invoke(CollisionInfo collision) => action.Invoke(collision.gameObject);
    }
}
