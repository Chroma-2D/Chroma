using Chroma.SDL2;

namespace Chroma.Graphics
{
    public class Display
    {
        internal SDL.SDL_DisplayMode UnderlyingDisplayMode { get; set; }

        public int Index { get; }
        public int RefreshRate { get; }
        public Size Dimensions { get; }

        internal Display(int index, int refreshRate, ushort width, ushort height)
        {
            Index = index;
            RefreshRate = refreshRate;
            Dimensions = new Size(width, height);
        }
    }
}
