using System.Collections.Generic;
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

        public List<DisplayMode> QuerySupportedDisplayModes()
        {
            var ret = new List<DisplayMode>();
            var displayModeCount = SDL2.SDL_GetNumDisplayModes(Index);
            
            for (var i = 0; i < displayModeCount; i++)
            {
                SDL2.SDL_GetDisplayMode(Index, i, out var mode);
                ret.Add(new DisplayMode(mode.w, mode.h, mode.refresh_rate));
            }

            return ret;
        }
    }
}