using UnityEngine;

namespace Slothsoft.Effects {
    public interface ITriggerHit {
        Vector2 pointSum { get; }
        int pointCount { get; }
    }
}
