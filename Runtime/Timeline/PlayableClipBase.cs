using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;

namespace Slothsoft.Events.Timeline {
    public abstract class PlayableClipBase<TSettings, TBehaviour, TTarget> : PlayableAsset, IClip<TTarget>, IExpandableClip
#if UNITY_EDITOR
        , ISerializationCallbackReceiver
#endif
        where TSettings : class, IPlayableAsset
        where TBehaviour : class, IPlayableBehaviour, ISettingsBehaviour<TSettings>, new()
        where TTarget : Object {

        public sealed override Playable CreatePlayable(PlayableGraph graph, GameObject owner) {
            var playable = ScriptPlayable<TBehaviour>.Create(graph);

            playable.GetBehaviour().SetUp(this as TSettings);

            return playable;
        }

        [Header("Custom Playable")]
        [SerializeField]
        bool m_inheritDuration = false;
        public bool inheritDuration => m_inheritDuration;

#if UNITY_EDITOR
        public virtual string displayName => GetType().Name;

        void ISerializationCallbackReceiver.OnBeforeSerialize() {
            if (!inheritDuration) {
                return;
            }

            string path = UnityEditor.AssetDatabase.GetAssetPath(this);
            if (string.IsNullOrEmpty(path)) {
                return;
            }

            if (UnityEditor.AssetDatabase.LoadMainAssetAtPath(path) is TimelineAsset timeline) {
                double duration = timeline.duration;
                foreach (var asset in timeline.GetOutputTracks()) {
                    foreach (var clip in asset.GetClips()) {
                        if (clip.asset == this) {
                            duration = clip.duration;
                            break;
                        }
                    }
                }

                timeline.SetAllClipDurations(duration);
            }
        }
        void ISerializationCallbackReceiver.OnAfterDeserialize() {
        }
#endif
    }
}
