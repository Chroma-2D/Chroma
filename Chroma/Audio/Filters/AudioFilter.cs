using System;

namespace Chroma.Audio.Filters
{
    public class AudioFilter : AudioObject
    {
        internal AudioFilter(IntPtr handle) : base(handle)
        {
             // todo abstract filters
        }
    }
}