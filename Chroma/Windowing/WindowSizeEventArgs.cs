using System;
using System.Drawing;

namespace Chroma.Windowing
{
    public class WindowSizeEventArgs : EventArgs
    {
        public Size Size { get; }

        internal WindowSizeEventArgs(Size size)
        {
            Size = size;
        }
    }
}