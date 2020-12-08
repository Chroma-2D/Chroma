using System;

namespace Chroma.Audio.Sources
{
    public class UserDefinedAudioSource : AudioSource
    {
        internal override void Initialize()
        {
        }

        internal override void SourceCallback(IntPtr userData, IntPtr stream, int streamSize)
        {
        }
    }
}