using System;
using UnityEngine;

namespace Slothsoft.Events {
    [Serializable]
    public sealed class CursedEvent {
        event Action onGlobal;
        event Action<GameObject> onGameObject;
        event Action<CollisionInfo> onCollision;

        [SerializeReference]
        public ICursedAction[] actions = Array.Empty<ICursedAction>();

        public bool hasPersistentListeners => actions.Length > 0;

        public void Invoke() {
            for (int i = 0; i < actions.Length; i++) {
                actions[i].Invoke();
            }

            onGlobal?.Invoke();
            onGameObject?.Invoke(null);
            onCollision?.Invoke(CollisionInfo.empty);
        }

        public void Invoke(GameObject gameObject) {
            for (int i = 0; i < actions.Length; i++) {
                actions[i].Invoke(gameObject);
            }

            onGlobal?.Invoke();
            onGameObject?.Invoke(gameObject);
            onCollision?.Invoke(new CollisionInfo(gameObject));
        }

        public void Invoke(CollisionInfo collision) {
            for (int i = 0; i < actions.Length; i++) {
                actions[i].Invoke(collision);
            }

            onGlobal?.Invoke();
            onGameObject?.Invoke(collision.gameObject);
            onCollision?.Invoke(collision);
        }

        public void AddListener(Action action) {
            onGlobal += action;
        }

        public void AddListener(Action<GameObject> action) {
            onGameObject += action;
        }

        public void AddListener(Action<CollisionInfo> action) {
            onCollision += action;
        }

        public void RemoveListener(Action action) {
            onGlobal -= action;
        }

        public void RemoveListener(Action<GameObject> action) {
            onGameObject -= action;
        }

        public void RemoveListener(Action<CollisionInfo> action) {
            onCollision -= action;
        }
    }
}
