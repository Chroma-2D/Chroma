using System.IO;
using Chroma.Diagnostics.Logging;

namespace Chroma.Audio.Sources
{
    public class Music : FileBasedAudioSource
    {
        private readonly Log _log = LogManager.GetForCurrentAssembly();

        public Music(Stream stream)
            : base(stream, false)
        {
        }

        public Music(string filePath)
            : base(filePath, false)
        {
        }
    }
}