using System;
using UnityEngine;

namespace Slothsoft.Effects.ObjectLocators {
    [Serializable]
    public sealed class TransformReference {
        [SerializeReference]
        internal ITransformLocator locator = new LocateFromContext();

        public bool TryGetTransform(GameObject context, out Transform transform) {
            return locator.TryLocate(context, out transform);
        }
    }
}
