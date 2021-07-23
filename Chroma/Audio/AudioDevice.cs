using Chroma.Natives.SDL;

namespace Chroma.Audio
{
    public class AudioDevice
    {
        public int Index { get; }
        public bool IsCapture { get; }
        public bool IsCurrentlyInUse { get; private set; }

        public string Name => SDL2.SDL_GetAudioDeviceName(Index, IsCapture);

        internal AudioDevice(int index, bool isCapture)
        {
            Index = index;
            IsCapture = isCapture;
        }

        internal void Lock()
        {
            IsCurrentlyInUse = true;
        }

        internal void Unlock()
        {
            IsCurrentlyInUse = false;
        }
    }
}