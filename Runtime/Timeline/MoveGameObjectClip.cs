using System.Text;
using Slothsoft.Effects.ObjectLocators;
using UnityEngine;
using UnityEngine.Playables;

namespace Slothsoft.Effects.Timeline {
    sealed class MoveGameObjectClip : PlayableClipBase<MoveGameObjectClip, MoveGameObjectClip.Behavior, GameObject> {
        public sealed class Behavior : PlayableBehaviorBase<MoveGameObjectClip, GameObject> {
            Transform context;
            float smoothTime;
            Vector3 velocity;

            protected override void OnStateEnter(in Playable playable, in FrameData info) {
                settings.moveContext.TryGetTransform(target, out context);
                smoothTime = (float)playable.GetDuration();
                velocity = Vector3.zero;
            }
            protected override void OnStateUpdate(in Playable playable, in FrameData info) {
                if (!context) {
                    return;
                }

                context.position = Vector3.SmoothDamp(
                    context.position,
                    settings.position,
                    ref velocity,
                    smoothTime,
                    info.deltaTime
                );
            }
            protected override void OnStateAbort(in Playable playable, in FrameData info) {
                if (!context) {
                    return;
                }
            }
            protected override void OnStateExit(in Playable playable, in FrameData info) {
                if (!context) {
                    return;
                }

                context.position = settings.position;
            }
        }

        [Header("Settings")]
        [SerializeField]
        internal TransformReference moveContext = new();
        [SerializeField]
        internal Vector3 position = Vector3.zero;

#if UNITY_EDITOR
        public override string displayName {
            get {
                var builder = new StringBuilder();
                builder.Append($"MoveTo{position}");
                return builder.ToString().Trim();
            }
        }
#endif
    }
}
