namespace Chroma.Audio.Sources;

using System.IO;

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