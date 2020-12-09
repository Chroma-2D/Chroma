using System.IO;

namespace Chroma.Audio.Sources
{
    public class Sound : FileBasedAudioSource
    {
        internal Sound(string filePath) 
            : base(filePath, true)
        {
        }

        internal Sound(Stream stream) 
            : base(stream, true)
        {
        }
    }
}