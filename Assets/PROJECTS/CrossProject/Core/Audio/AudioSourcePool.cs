using CrossProject.Core.Pooling;

namespace CrossProject.Core.Audio{
    public class AudioSourcePool : MonoPool<AudioSourcePoolElement>
    {
        public void ToggleMute()
        {
            foreach (var poolElement in ActiveElements)
                poolElement.Source.mute = !poolElement.Source.mute;
        }
    }
}