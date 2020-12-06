using Chroma.Natives.SDL;

namespace Chroma.Audio
{
    public class AudioDeviceEventArgs
    {
        public uint Index { get; }
        public bool IsCapture { get; }

        public string Name => SDL2.SDL_GetAudioDeviceName(
            (int)Index, 
            IsCapture
        );

        internal AudioDeviceEventArgs(uint index, bool isCapture)
        {
            Index = index;
            IsCapture = isCapture;
        }
    }
}