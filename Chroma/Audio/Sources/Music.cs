using System;
using System.IO;

namespace Chroma.Audio.Sources
{
    public sealed class Music : FileBasedAudioSource
    {
        public Music(string filePath)
            : base(filePath, false)
        {
            if (Handle == IntPtr.Zero)
                return;
            
            NotifyInitializationFinished();
        }
        
        public Music(Stream stream)
            : base(stream, false)
        {
            if (Handle == IntPtr.Zero)
                return;
            
            NotifyInitializationFinished();
        }
    }
}