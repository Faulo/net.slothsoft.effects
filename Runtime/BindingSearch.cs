using UnityEngine;
using UnityEngine.Playables;
using UnityObject = UnityEngine.Object;

namespace Slothsoft.Events {
    public delegate bool BindingSearch(GameObject instance, in PlayableBinding binding, out UnityObject result);
}