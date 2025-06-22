using UnityEngine.Playables;

namespace Slothsoft.Effects.Timeline {
    public interface ISettingsBehaviour<in TSettings>
        where TSettings : IPlayableAsset {

        void SetUp(TSettings settings);
    }
}
