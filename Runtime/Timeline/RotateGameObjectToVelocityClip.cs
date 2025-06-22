using System.Text;
using UnityEngine;
using UnityEngine.Playables;

namespace Slothsoft.Events.Timeline {
    sealed class RotateGameObjectToVelocityClip : PlayableClipBase<RotateGameObjectToVelocityClip, RotateGameObjectToVelocityClip.Behavior, GameObject> {
        public sealed class Behavior : PlayableBehaviorBase<RotateGameObjectToVelocityClip, GameObject> {
            Vector3 previousPosition;
            Vector3 nextPosition;
            Vector3 velocity;

            protected override void OnStateEnter(in Playable playable, in FrameData info) {
                previousPosition = target.transform.position;
            }
            protected override void OnStateUpdate(in Playable playable, in FrameData info) {
                nextPosition = target.transform.position;
                Vector3.SmoothDamp(
                    previousPosition,
                    nextPosition,
                    ref velocity,
                    settings.rotationSmoothing,
                    info.deltaTime
                );

                if (velocity != Vector3.zero) {
                    target.transform.rotation = Quaternion.LookRotation(velocity.normalized, Vector3.forward);
                }

                previousPosition = nextPosition;
            }
            protected override void OnStateAbort(in Playable playable, in FrameData info) {
            }
            protected override void OnStateExit(in Playable playable, in FrameData info) {
            }
        }

        [Header("Settings")]
        [SerializeField]
        internal float rotationSmoothing = 1;

#if UNITY_EDITOR
        public override string displayName {
            get {
                var builder = new StringBuilder();
                builder.Append($"RotateToVelocity({rotationSmoothing})");
                return builder.ToString().Trim();
            }
        }
#endif
    }
}
