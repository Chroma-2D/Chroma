using System;
using System.IO;

namespace Chroma.Audio.Sources
{
    public sealed class Sound : FileBasedAudioSource
    {
        public Sound(string filePath)
            : base(filePath, true)
        {
            if (Handle == IntPtr.Zero)
                return;

            NotifyInitializationFinished();
        }

        public Sound(Stream stream)
            : base(stream, true)
        {
            if (Handle == IntPtr.Zero)
                return;
            
            NotifyInitializationFinished();
        }
    }
}