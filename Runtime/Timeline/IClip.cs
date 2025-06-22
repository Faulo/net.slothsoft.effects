namespace Slothsoft.Effects.Timeline {
    public interface IClip<out T> {
#if UNITY_EDITOR
        string displayName { get; }
#endif
    }
}
