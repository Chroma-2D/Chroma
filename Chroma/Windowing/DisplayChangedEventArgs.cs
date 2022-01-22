using System;

namespace Chroma.Windowing
{
    public class DisplayChangedEventArgs : EventArgs
    {
        public int Index { get; }

        internal DisplayChangedEventArgs(int index)
        {
            Index = index;
        }
    }
}