using Chroma.Audio.Sources;

namespace Chroma.Audio
{
    public class AudioSourceEventArgs
    {
        public AudioSource Source { get; }
        public bool IsLooping { get; }

        internal AudioSourceEventArgs(AudioSource source, bool isLooping)
        {
            Source = source;
            IsLooping = isLooping;
        }
    }
}