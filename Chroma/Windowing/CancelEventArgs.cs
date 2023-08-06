using System;

namespace Chroma.Windowing
{
    public sealed class CancelEventArgs
    {
        public bool Cancel { get; set; }

        internal CancelEventArgs()
        {
        }
    }
}