namespace Chroma.Windowing.EventArgs
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
