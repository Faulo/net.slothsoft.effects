using System.Text;
using MyBox;
using Slothsoft.Effects.ObjectLocators;
using UnityEngine;
using UnityEngine.Playables;

namespace Slothsoft.Effects.Timeline {
    sealed class InstantiatePrefabClip : PlayableClipBase<InstantiatePrefabClip, InstantiatePrefabClip.Behavior, GameObject> {
        public sealed class Behavior : PlayableBehaviorBase<InstantiatePrefabClip, GameObject> {
            GameObject instance;
            ParticleSystem particleSystemInstance;
            protected override void OnStateEnter(in Playable playable, in FrameData info) {
                if (!settings.instantiateAt.TryGetTransform(target, out var parent)) {
                    Debug.LogError($"Failed to find transform for {this}", settings);
                    return;
                }

                SetUp(parent);
            }
            protected override void OnStateUpdate(in Playable playable, in FrameData info) {
            }
            protected override void OnStateAbort(in Playable playable, in FrameData info) {
                if (settings.destroyOnAbort) {
                    CleanUp();
                }
            }
            protected override void OnStateExit(in Playable playable, in FrameData info) {
                if (settings.destroyOnExit) {
                    CleanUp();
                }
            }
            void SetUp(Transform context) {
                var parent = settings.parentToContext
                    ? context
                    : null;

                if (settings.isParticleSystem) {
                    particleSystemInstance = Instantiate(settings.particleSystemPrefab, context.position, context.rotation, parent);
                } else {
                    instance = Instantiate(settings.prefab, context.position, context.rotation, parent);
                }
            }
            void CleanUp() {
                if (settings.isParticleSystem) {
                    if (!particleSystemInstance) {
                        return;
                    }

                    if (settings.useDestructionDelay) {
                        Debug.LogWarning($"{nameof(useDestructionDelay)} is not supported for particle systems!", settings);
                    }

                    var emission = particleSystemInstance.emission;
                    emission.enabled = false;
                } else {
                    if (!instance) {
                        return;
                    }

                    if (settings.useDestructionDelay) {
                        Destroy(instance, settings.destructionDelay);
                    } else {
                        Destroy(instance);
                    }
                }
            }
        }
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
        internal TransformReference instantiateAt = new();
        [Space]
        [SerializeField]
        internal bool destroyOnAbort = false;
        [SerializeField]
        internal bool destroyOnExit = false;
        [SerializeField]
        internal bool useDestructionDelay = false;
        [SerializeField, ConditionalField(nameof(useDestructionDelay)), Range(0, 10)]
        internal float destructionDelay = 0;

        void OnValidate() {
            isParticleSystem = prefab && prefab.TryGetComponent(out particleSystemPrefab);
        }

#if UNITY_EDITOR
        public override string displayName {
            get {
                var builder = new StringBuilder();
                if (prefab) {
                    builder.Append($"Instantiate({prefab.name})");
                } else {
                    builder.Append("InstantiatePrefab");
                }

                return builder.ToString().Trim();
            }
        }
#endif
    }
}
