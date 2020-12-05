namespace Chroma.Audio
{
    public class AudioDeviceEventArgs
    {
        public uint Index { get; }
        public bool IsCapture { get; }

        internal AudioDeviceEventArgs(uint index, bool isCapture)
        {
            Index = index;
            IsCapture = isCapture;
        }
    }
}