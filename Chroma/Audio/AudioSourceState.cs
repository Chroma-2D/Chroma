using Chroma.Natives.OpenAL;

namespace Chroma.Audio
{
    public enum AudioSourceState
    {
        Invalid = -1,
        Initial = Al.AL_INITIAL,
        Stopped = Al.AL_STOPPED,
        Playing = Al.AL_PLAYING,
        Paused = Al.AL_PAUSED
    }
}