using Chroma.Natives.SDL;

namespace Chroma.Audio
{
    public class AudioDevice
    {
        public int Index { get; }
        public bool IsCapture { get; }

        public string Name => SDL2.SDL_GetAudioDeviceName(Index, IsCapture);

        internal AudioDevice(int index, bool isCapture)
        {
            Index = index;
            IsCapture = isCapture;
        }
    }
}