using System.Drawing;

namespace Chroma.Windowing
{
    public sealed class WindowSizeEventArgs
    {
        public Size Size { get; }

        internal WindowSizeEventArgs(Size size)
        {
            Size = size;
        }
    }
}