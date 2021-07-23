using System;

namespace Chroma.Audio
{
    public class AudioCaptureEventArgs : EventArgs
    {
        public byte[] Buffer { get; }

        internal AudioCaptureEventArgs(byte[] buffer)
        {
            Buffer = buffer;
        }
    }
}