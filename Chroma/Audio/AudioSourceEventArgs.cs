using Chroma.Audio.Sources;

namespace Chroma.Audio
{
    public class AudioSourceEventArgs
    {
        public AudioSource Source { get; }

        internal AudioSourceEventArgs(AudioSource source)
            => Source = source;
    }
}