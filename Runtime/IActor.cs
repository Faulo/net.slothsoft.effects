using UnityEngine;

namespace Slothsoft.Events {
    public interface IActor {
        /// <summary>
        /// This actor's <see cref="GameObject"/>, if any.
        /// </summary>
        GameObject gameObject { get; }

        /// <summary>
        /// Is this actor currently defying gravity?
        /// </summary>
        bool isFlying { get; }

        /// <summary>
        /// Is this actor currently airborne and moving downwards?
        /// </summary>
        bool isFalling { get; }

        /// <summary>
        /// Is this actor currently resting on another collider?
        /// </summary>
        bool isGrounded { get; }

        /// <summary>
        /// The current velocity of this actor.
        /// </summary>
        Vector2 velocity { get; }

        /// <summary>
        /// Force-move this actor by an offset.
        /// </summary>
        /// <param name="delta"></param>
        void MoveBy(Vector2 delta);
    }
}
