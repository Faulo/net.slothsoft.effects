using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;

namespace Slothsoft.Events {
    [CreateAssetMenu(menuName = AssemblyInfo.MENU_EVENTS + nameof(CursedTimeline))]
    sealed class CursedTimeline : TimelineAsset {
        public void PlayOneShot(CollisionInfo collision) {
            var obj = new GameObject(name);
            obj.transform.position = collision.point;
            obj.SetActive(false);
            obj.AddComponent<PlayableDirector>().playableAsset = this;
            obj.SetActive(true);
        }
    }
}
