using System;
using UnityEngine;

namespace Slothsoft.Effects.Triggers {
    public abstract class EffectTriggerBase<T> : MonoBehaviour where T : struct, Enum {
        [Serializable]
        internal class Entry {
            /// <summary>
            /// What type of event is the associated callback listening for.
            /// </summary>
            [SerializeField]
            internal T eventID = default;

            /// <summary>
            /// The desired <see cref="EffectEvent"/> to be Invoke'd.
            /// </summary>
            [SerializeReference]
            internal EffectEvent callback = new();
        }

        [SerializeField, HideInInspector]
        internal Entry[] entries = Array.Empty<Entry>();

        bool TryGetValue(T id, out EffectEvent callback) {
            for (int i = 0; i < entries.Length; i++) {
                if (entries[i].eventID.Equals(id)) {
                    callback = entries[i].callback;
                    return callback is not null;
                }
            }

            callback = null;
            return false;
        }

        protected void Execute(T id) {
            if (TryGetValue(id, out var callback)) {
                callback.Invoke();
            }
        }

        protected void Execute(T id, GameObject obj) {
            if (TryGetValue(id, out var callback)) {
                callback.Invoke(obj);
            }
        }

        protected void Execute(T id, CollisionInfo collision) {
            if (TryGetValue(id, out var callback)) {
                callback.Invoke(collision);
            }
        }
    }
}