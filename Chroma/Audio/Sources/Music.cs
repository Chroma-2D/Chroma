using System.IO;

namespace Chroma.Audio.Sources
{
    public class Music : FileBasedAudioSource
    {
        public Music(string filePath)
            : base(filePath, false)
        {
        }
        
        public Music(Stream stream)
            : base(stream, false)
        {
        }
    }
}