namespace Chroma.Audio
{
    public class SoundEventArgs
    {
        public Sound Sound { get; }

        internal SoundEventArgs(Sound sound)
        {
            Sound = sound;
        }
    }
}