using Chroma.Natives.SDL;

namespace Chroma.Graphics
{
    public class Display
    {
        internal SDL2.SDL_DisplayMode UnderlyingDisplayMode { get; set; }

        public int Index { get; }
        public int RefreshRate { get; }
        public ushort Width { get; }
        public ushort Height { get; }

        internal Display(int index, int refreshRate, ushort width, ushort height)
        {
            Index = index;
            RefreshRate = refreshRate;
            Width = width;
            Height = height;
        }
    }
}