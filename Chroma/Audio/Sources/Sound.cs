using System.IO;

namespace Chroma.Audio.Sources
{
    public class Sound : FileBasedAudioSource
    {
        public Sound(string filePath) 
            : base(filePath, true)
        {
        }

        public Sound(Stream stream) 
            : base(stream, true)
        {
        }
    }
}