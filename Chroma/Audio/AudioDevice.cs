using Chroma.Natives.SDL;

namespace Chroma.Audio
{
    public class AudioDevice
    {
        internal uint OpenIndex { get; private set; }
        public int Index { get; }
        
        public bool IsCapture { get; }
        public bool IsCurrentlyInUse => OpenIndex != 0;

        public string Name => SDL2.SDL_GetAudioDeviceName(Index, IsCapture);

        internal AudioDevice(int index, bool isCapture)
        {
            Index = index;
            IsCapture = isCapture;
        }
        
        internal void Lock(uint openIndex)
        {
            OpenIndex = openIndex;
        }

        internal void Unlock()
        {
            OpenIndex = 0;
        }
    }
}