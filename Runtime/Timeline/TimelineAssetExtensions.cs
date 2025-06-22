using UnityEngine.Timeline;

namespace Slothsoft.Effects.Timeline {
    public static class TimelineAssetExtensions {
        public static void SetAllClipDurations(this TimelineAsset timeline, double duration) {
            foreach (var track in timeline.GetRootTracks()) {
                foreach (var clip in track.GetClips()) {
                    if (clip.asset is IExpandableClip playable && playable.inheritDuration) {
                        clip.timeScale = 1;
                        clip.start = 0;
                        clip.duration = duration;
                    }
                }
            }
        }
        public static bool TryGetClip<T>(this TimelineAsset timeline, out T clipAsset)
            where T : UnityEngine.Object {
            foreach (var track in timeline.GetRootTracks()) {
                foreach (var clip in track.GetClips()) {
                    clipAsset = clip.asset as T;
                    if (clipAsset != default) {
                        return true;
                    }
                }
            }

            clipAsset = default;
            return false;
        }
    }
}
