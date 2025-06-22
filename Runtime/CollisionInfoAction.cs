using System;
using Slothsoft.UnityExtensions;
using UnityEngine;
using UnityEngine.Events;

namespace Slothsoft.Events {
    [ImplementationFor(typeof(ICursedAction), nameof(CollisionInfoAction))]
    [Serializable]
    sealed class CollisionInfoAction : ICursedAction {
        [SerializeField]
        UnityEvent<CollisionInfo> action = new();

        public void Invoke() => action.Invoke(default);
        public void Invoke(GameObject context) => action.Invoke(new(context));
        public void Invoke(CollisionInfo collision) => action.Invoke(collision);
    }
}
