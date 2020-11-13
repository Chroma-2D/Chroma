using System;

namespace Chroma.Windowing
{
    public class CancelEventArgs : EventArgs
    {
        public bool Cancel { get; set; }

        internal CancelEventArgs()
        {
        }
    }
}