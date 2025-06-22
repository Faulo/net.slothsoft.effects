using System.ComponentModel;
using UnityEngine;
using UnityEngine.Timeline;

namespace Slothsoft.Events.Timeline {
    [TrackClipType(typeof(IClip<GameObject>))]
    [TrackBindingType(typeof(GameObject))]
    [TrackColor(0.2762405f, 0.695f, 0.139f)]
    [DisplayName("GameObject Track")]
    public sealed class GameObjectTrack : TrackBase {
    }
}
