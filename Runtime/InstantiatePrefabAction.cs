using System;
using MyBox;
using Slothsoft.Events.ObjectLocators;
using Slothsoft.UnityExtensions;
using UnityEngine;
using UnityObject = UnityEngine.Object;

namespace Slothsoft.Events {
    [ImplementationFor(typeof(ICursedAction), nameof(InstantiatePrefabAction))]
    [Serializable]
    sealed class InstantiatePrefabAction : ICursedAction, ISerializationCallbackReceiver {
        [Header("Settings")]
        [SerializeField]
        internal GameObject prefab;
        [SerializeField, ReadOnly]
        internal bool isParticleSystem = false;
        [SerializeField, ReadOnly, ConditionalField(nameof(isParticleSystem))]
        internal ParticleSystem particleSystemPrefab = default;
        [Space]
        [SerializeField]
        internal bool parentToContext = false;
        [SerializeField]
        internal bool useCollisionPositionIfAvailable = false;
        [SerializeField]
        internal TransformReference instantiateAt = new();

        [Space]
        [SerializeField]
        internal bool useDestructionDelay = false;
        [SerializeField, ConditionalField(nameof(useDestructionDelay)), Range(0, 10)]
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

            if (isParticleSystem) {
                var instance = UnityObject.Instantiate(particleSystemPrefab, position, rotation, parent);
                if (useDestructionDelay) {
                    UnityObject.Destroy(instance.gameObject, destructionDelay);
                }
            } else {
                var instance = UnityObject.Instantiate(prefab, position, rotation, parent);
                if (useDestructionDelay) {
                    UnityObject.Destroy(instance, destructionDelay);
                }
            }
        }

        public void OnBeforeSerialize() {
            Validate();
        }
        public void OnAfterDeserialize() {
        }

        void Validate() {
            isParticleSystem = prefab && prefab.TryGetComponent(out particleSystemPrefab);
            if (!isParticleSystem) {
                particleSystemPrefab = null;
            }
        }
    }
}
