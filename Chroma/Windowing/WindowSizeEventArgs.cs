using System.Drawing;

namespace Chroma.Windowing
{
    public class WindowSizeEventArgs : System.EventArgs
    {
        public Size Size { get; }

        internal WindowSizeEventArgs(Size size)
        {
            Size = size;
        }
    }
}