using System;
using Slothsoft.UnityExtensions;
using UnityEngine;

namespace Slothsoft.Events.ObjectLocators {
    [ImplementationFor(typeof(ITransformLocator), "Use Transform from Scene")]
    [Serializable]
    sealed class LocateInScene : ITransformLocator {
        [SerializeField]
        internal Transform transform;

        public bool TryLocate(GameObject context, out Transform result) {
            result = transform;
            return result;
        }
    }
}
