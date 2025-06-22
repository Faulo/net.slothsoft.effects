using System.Text;
using Slothsoft.Effects.ObjectLocators;
using UnityEngine;
using UnityEngine.Playables;

namespace Slothsoft.Effects.Timeline {
    sealed class ToggleGameObjectClip : PlayableClipBase<ToggleGameObjectClip, ToggleGameObjectClip.Behavior, GameObject> {
        public sealed class Behavior : PlayableBehaviorBase<ToggleGameObjectClip, GameObject> {
            protected override void OnStateEnter(in Playable playable, in FrameData info) {
                if (settings.toggleContext.TryGetTransform(target, out var context)) {
                    context.gameObject.SetActive(!context.gameObject.activeSelf);
                }
            }
            protected override void OnStateUpdate(in Playable playable, in FrameData info) {
            }
            protected override void OnStateAbort(in Playable playable, in FrameData info) {
            }
            protected override void OnStateExit(in Playable playable, in FrameData info) {
            }
        }

        [Header("Settings")]
        [SerializeField]
        internal TransformReference toggleContext = new();

#if UNITY_EDITOR
        public override string displayName {
            get {
                var builder = new StringBuilder();
                builder.Append($"Toggle");
                return builder.ToString().Trim();
            }
        }
#endif
    }
}
