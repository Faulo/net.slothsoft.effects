using UnityEngine;

namespace Slothsoft.Events.ObjectLocators {
    public interface ITransformLocator {
        bool TryLocate(GameObject context, out Transform result);
    }
}
