using System;
using MyBox;
using Slothsoft.UnityExtensions;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Slothsoft.Effects.Effects {
    [ImplementationFor(typeof(IEffect), nameof(LoadScene))]
    [Serializable]
    sealed class LoadScene : IEffect {
        [SerializeField]
        SceneReference scene = new();
        [SerializeField]
        LoadSceneMode mode = LoadSceneMode.Single;
        [SerializeField]
        bool loadAsync = true;

        public void Invoke() {
            if (loadAsync) {
                scene.LoadSceneAsync(mode);
            } else {
                scene.LoadScene(mode);
            }
        }
        public void Invoke(GameObject context) => Invoke();
        public void Invoke(CollisionInfo collision) => Invoke();
    }
}
