namespace Chroma.Audio.Sources;

using System.IO;

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