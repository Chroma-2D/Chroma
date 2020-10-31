namespace Chroma.Graphics
{
    public class DisplayMode
    {
        public int Width { get; }
        public int Height { get; }
        public int RefreshRate { get; }

        internal DisplayMode(int width, int height, int refreshRate)
        {
            Width = width;
            Height = height;
            RefreshRate = refreshRate;
        }
    }
}