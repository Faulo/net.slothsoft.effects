using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Timeline;

namespace Slothsoft.Events.Timeline {
    public abstract class TrackBase : PlayableTrack {
#if UNITY_EDITOR
        protected override void OnBeforeTrackSerialize() {
            base.OnBeforeTrackSerialize();
            if (!this) {
                return;
            }

            var names = new HashSet<string>();
            foreach (var clip in GetClips()) {
                names.Add(clip.asset.GetType().Name);
                if (clip.asset is IClip<Object> clipAsset) {
                    clip.displayName = clipAsset.displayName;
                }
            }

            name = names.Count > 0
                ? $"{GetType().Name} ({string.Join(", ", names)})"
                : GetType().Name;
        }
#endif
    }
}
