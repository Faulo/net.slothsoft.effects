using UnityEngine.Playables;

namespace Slothsoft.Events.Timeline {
    public interface ISettingsBehaviour<in TSettings>
        where TSettings : IPlayableAsset {

        void SetUp(TSettings settings);
    }
}
