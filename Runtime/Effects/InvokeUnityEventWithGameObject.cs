using System;
using Slothsoft.UnityExtensions;
using UnityEngine;
using UnityEngine.Events;

namespace Slothsoft.Effects.Effects {
    [ImplementationFor(typeof(IEffect), nameof(InvokeUnityEventWithGameObject))]
    [Serializable]
    sealed class InvokeUnityEventWithGameObject : IEffect {
        [SerializeField]
        UnityEvent<GameObject> onInvoke = new();

        public void Invoke() => onInvoke.Invoke(default);
        public void Invoke(GameObject context) => onInvoke.Invoke(context);
        public void Invoke(CollisionInfo collision) => onInvoke.Invoke(collision.gameObject);
    }
}
