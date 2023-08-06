namespace Chroma.Audio
{
    public sealed class AudioCaptureEventArgs
    {
        public byte[] Buffer { get; }

        internal AudioCaptureEventArgs(byte[] buffer)
        {
            Buffer = buffer;
        }
    }
}