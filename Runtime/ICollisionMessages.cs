namespace Slothsoft.Effects {
    public interface ICollisionMessages {
        /// <summary>
        /// Gets called any time this actor enters a collider or trigger.
        /// </summary>
        /// <param name="collision"></param>
        void OnAvatarEnter(CollisionInfo collision);

        /// <summary>
        /// Gets called any time this actor exits a trigger.
        /// </summary>
        /// <param name="collision"></param>
        void OnAvatarExit(CollisionInfo collision);
    }
}