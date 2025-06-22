using UnityEngine;
namespace Slothsoft.Events {
    public interface ITriggerHit {
        Vector2 pointSum { get; }
        int pointCount { get; }
    }
}
