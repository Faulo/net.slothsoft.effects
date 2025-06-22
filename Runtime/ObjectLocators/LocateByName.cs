using System;
using System.Linq;
using Slothsoft.UnityExtensions;
using UnityEngine;

namespace Slothsoft.Effects.ObjectLocators {
    [ImplementationFor(typeof(ITransformLocator), "Find GameObject by name")]
    [Serializable]
    sealed class LocateByName : ITransformLocator {
        [SerializeField]
        internal string name;

        public bool TryLocate(GameObject context, out Transform result) {
            result = Resources
                .FindObjectsOfTypeAll<Transform>()
                .FirstOrDefault(transform => transform.name == name);
            return result;
        }
    }
}
