using UnityEngine;
using UnityEngine.Playables;

namespace Slothsoft.Events.Timeline {
    sealed class CursedActionClip : PlayableClipBase<CursedActionClip, CursedActionClip.Behavior, GameObject> {
        public sealed class Behavior : PlayableBehaviorBase<CursedActionClip, GameObject> {
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
            void RaiseEffects(CursedEvent eve) {
                if (!eve.hasPersistentListeners) {
                    return;
                }

                eve.Invoke(target);
            }
        }

        [Header("Settings")]
        [SerializeField]
        CursedEvent onStateEnter = new();
        [SerializeField]
        CursedEvent onStateAbort = new();
        [SerializeField]
        CursedEvent onStateExit = new();

#if UNITY_EDITOR
        public override string displayName {
            get {
                return nameof(CursedActionClip);
            }
        }
#endif
    }
}