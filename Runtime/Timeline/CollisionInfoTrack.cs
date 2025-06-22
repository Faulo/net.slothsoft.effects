using System.ComponentModel;
using UnityEngine.Timeline;

namespace Slothsoft.Events.Timeline {
    [TrackClipType(typeof(IClip<CollisionInfo>))]
    [TrackBindingType(typeof(CollisionInfo))]
    [TrackColor(0.2762405f, 0.695f, 0.139f)]
    [DisplayName("CollisionInfo Track")]
    sealed class CollisionInfoTrack : TrackBase {
    }
}
