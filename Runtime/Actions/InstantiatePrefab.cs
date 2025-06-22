using System;
using MyBox;
using Slothsoft.Effects.ObjectLocators;
using Slothsoft.UnityExtensions;
using UnityEngine;
using UnityObject = UnityEngine.Object;

namespace Slothsoft.Effects.Effects {
    [ImplementationFor(typeof(IEffect), nameof(InstantiatePrefab))]
    [Serializable]
    sealed class InstantiatePrefab : IEffect {
        [SerializeField]
        internal GameObject prefab;

        [Space]
        [SerializeField]
        internal bool parentToContext = false;
        [SerializeField]
        internal bool useCollisionPositionIfAvailable = false;
        [SerializeField]
        internal TransformReference instantiateAt = new();

        [Space]
        [SerializeField]
        [Tooltip("Destroy the instantiated prefab after x seconds")]
        internal bool useDestructionDelay = false;
        [SerializeField, ConditionalField(nameof(useDestructionDelay))]
        internal float destructionDelay = 0;

        public void Invoke() => SetUp(default);
        public void Invoke(GameObject context) => SetUp(context);
        public void Invoke(CollisionInfo collision) {
            if (useCollisionPositionIfAvailable) {
                if (TryGetContext(collision.gameObject, out var context)) {
                    Spawn(collision.point, collision.normalRotation, context);
                }
            } else {
                SetUp(collision.gameObject);
            }
        }

        void SetUp(GameObject gameObject) {
            if (TryGetContext(gameObject, out var context)) {
                Spawn(context.position, context.rotation, context);
            }
        }

        bool TryGetContext(GameObject gameObject, out Transform context) {
            if (instantiateAt.TryGetTransform(gameObject, out context)) {
                return true;
            }

            Debug.LogError($"Failed to find transform for {this}");
            return false;
        }

        void Spawn(in Vector3 position, in Quaternion rotation, Transform context) {
            var parent = context && parentToContext
                ? context
                : null;

            var instance = UnityObject.Instantiate(prefab, position, rotation, parent);
            if (useDestructionDelay) {
                UnityObject.Destroy(instance, destructionDelay);
            }
        }
    }
}
