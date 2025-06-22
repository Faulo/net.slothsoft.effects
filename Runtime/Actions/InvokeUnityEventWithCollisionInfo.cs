using System;
using Slothsoft.UnityExtensions;
using UnityEngine;
using UnityEngine.Events;

namespace Slothsoft.Events.Effects {
    [ImplementationFor(typeof(IEffect), nameof(InvokeUnityEventWithCollisionInfo))]
    [Serializable]
    sealed class InvokeUnityEventWithCollisionInfo : IEffect {
        [SerializeField]
        UnityEvent<CollisionInfo> onInvoke = new();

        public void Invoke() => onInvoke.Invoke(default);
        public void Invoke(GameObject context) => onInvoke.Invoke(new(context));
        public void Invoke(CollisionInfo collision) => onInvoke.Invoke(collision);
    }
}
