using UnityEngine;
using UnityEngine.Playables;

namespace Slothsoft.Effects.Timeline {
    sealed class EffectClip : PlayableClipBase<EffectClip, EffectClip.Behavior, GameObject> {
        public sealed class Behavior : PlayableBehaviorBase<EffectClip, GameObject> {
            protected override void OnStateEnter(in Playable playable, in FrameData info) {
                RaiseEffects(settings.onStateEnter);
            }
            protected override void OnStateUpdate(in Playable playable, in FrameData info) {
            }
            protected override void OnStateAbort(in Playable playable, in FrameData info) {
                RaiseEffects(settings.onStateAbort);
            }
            protected override void OnStateExit(in Playable playable, in FrameData info) {
                RaiseEffects(settings.onStateExit);
            }
            void RaiseEffects(EffectEvent eve) {
                if (!eve.hasPersistentListeners) {
                    return;
                }

                eve.Invoke(target);
            }
        }

        [Header("Settings")]
        [SerializeField]
        EffectEvent onStateEnter = new();
        [SerializeField]
        EffectEvent onStateAbort = new();
        [SerializeField]
        EffectEvent onStateExit = new();

#if UNITY_EDITOR
        public override string displayName {
            get {
                return nameof(EffectClip);
            }
        }
#endif
    }
}