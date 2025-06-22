using UnityEngine;

namespace Slothsoft.Effects.ObjectLocators {
    public interface ITransformLocator {
        bool TryLocate(GameObject context, out Transform result);
    }
}
