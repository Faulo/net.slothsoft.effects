using System;
using UnityEngine;

namespace Slothsoft.Events {
    sealed class TestComponent : MonoBehaviour {
        internal static event Action<Vector3, Quaternion> onAwakeTransform;
        internal static event Action<GameObject> onAwakeComponent;

        void Awake() {
            onAwakeComponent?.Invoke(gameObject);
            onAwakeTransform?.Invoke(transform.position, transform.rotation);
        }
    }
}
