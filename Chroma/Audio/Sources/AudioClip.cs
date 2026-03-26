namespace Chroma.Audio.Sources;

using System.IO;

public class AudioClip : FileBasedAudioSource
{
    public AudioClip(string filePath, bool decodeWhole) 
        : base(filePath, decodeWhole)
    {
    }

    public AudioClip(Stream stream, bool decodeWhole) 
        : base(stream, decodeWhole)
    {
    }
}