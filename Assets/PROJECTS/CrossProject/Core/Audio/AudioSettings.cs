namespace CrossProject.Core
{
    public interface IAudioSettings
    {
        public float SoundVolume { get; }
        public float MusicVolume { get; }
    }

    public class AudioSettings : IAudioSettings
    {
        public float SoundVolume { get; } = 1;
        public float MusicVolume { get; } = 1;
    }
}
