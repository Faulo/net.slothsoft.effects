using UnityEngine;
using UnityEngine.Playables;

namespace Slothsoft.Events.Timeline {
    public abstract class PlayableBehaviorBase<TSettings, TTarget> : PlayableBehaviour, ISettingsBehaviour<TSettings>
        where TSettings : IPlayableAsset
        where TTarget : Object {
        enum PlayableState {
            Unknown,
            FirstFrame,
            Running,
            Done,
        }

        public TSettings settings { get; private set; }
        protected TTarget target;
        protected double time;
        protected double duration;
        protected float normalizedTime => (float)(time / duration);

        PlayableDirector director;
        PlayableState state = PlayableState.Unknown;

        public virtual void SetUp(TSettings settings) {
            this.settings = settings;
        }

        public override void OnBehaviourPlay(Playable playable, FrameData info) {
            state = PlayableState.FirstFrame;
        }

        public override void ProcessFrame(Playable playable, FrameData info, object playerData) {
            switch (state) {
                case PlayableState.FirstFrame:
                    state = PlayableState.Running;

                    target = playerData as TTarget;
                    if (!target) {
                        Debug.LogWarning($"Failed to find context of type {typeof(TTarget)} for playable {this}!");
                        return;
                    }

                    time = 0;
                    duration = playable.GetDuration();
                    director = playable.GetGraph().GetResolver() as PlayableDirector;
                    OnStateEnter(playable, info);
                    break;

                case PlayableState.Running:
                    if (target) {
                        time += info.deltaTime * info.effectiveSpeed;
                        if (time < duration || director.extrapolationMode == DirectorWrapMode.Loop) {
                            OnStateUpdate(playable, info);
                        } else {
                            state = PlayableState.Done;
                            OnStateExit(playable, info);
                        }
                    }

                    break;
            }
#if UNITY_EDITOR
            OnDrawGizmos();
#endif
        }

#if UNITY_EDITOR
        protected virtual void OnDrawGizmos() {
        }
#endif

        public override void OnBehaviourPause(Playable playable, FrameData info) {
            switch (state) {
                case PlayableState.Running:
                    if (target) {
                        state = PlayableState.Done;

                        time += info.deltaTime * info.effectiveSpeed;
                        if (time >= duration) {
                            OnStateExit(playable, info);
                        } else {
                            OnStateAbort(playable, info);
                        }
                    }

                    break;
            }
        }
        protected abstract void OnStateEnter(in Playable playable, in FrameData info);
        protected abstract void OnStateUpdate(in Playable playable, in FrameData info);
        protected abstract void OnStateExit(in Playable playable, in FrameData info);
        protected abstract void OnStateAbort(in Playable playable, in FrameData info);
    }
}
