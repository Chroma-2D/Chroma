namespace Chroma.Windowing.EventArgs
{
    public class WindowResizeEventArgs : System.EventArgs
    {
        public Size Size { get; }

        internal WindowResizeEventArgs(Size size)
        {
            Size = size;
        }
    }
}
