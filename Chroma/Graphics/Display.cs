using Chroma.SDL2;

namespace Chroma.Graphics
{
    public class Display
    {
        internal SDL.SDL_DisplayMode UnderlyingDisplayMode { get; set; }

        public int Index { get; }
        public int RefreshRate { get; }
        public float Width { get; }
        public float Height { get; }

        internal Display(int index, int refreshRate, ushort width, ushort height)
        {
            Index = index;
            RefreshRate = refreshRate;
            Width = width;
            Height = height;
        }
    }
}
