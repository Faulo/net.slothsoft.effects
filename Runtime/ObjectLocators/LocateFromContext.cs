using System;
using Slothsoft.UnityExtensions;
using UnityEngine;

namespace Slothsoft.Effects.ObjectLocators {
    [ImplementationFor(typeof(ITransformLocator), "Use context GameObject", -1)]
    [Serializable]
    sealed class LocateFromContext : ITransformLocator {
        public bool TryLocate(GameObject context, out Transform result) {
            if (!context || !context.transform) {
                result = default;
                return false;
            }

            result = context.transform;
            return true;
        }
    }
}
