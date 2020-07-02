namespace Chroma.Windowing.EventArgs
{
    public class WindowSizeEventArgs : System.EventArgs
    {
        public float Width { get; }
        public float Height { get; }

        internal WindowSizeEventArgs(float width, float height)
        {
            Width = width;
            Height = height;
        }
    }
}