using System.Text;
using Chroma.Natives.SDL;

namespace Chroma.Graphics
{
    public class DisplayMode
    {
        public static readonly DisplayMode Invalid = new(-1, -1, -1);

        public int Width { get; }
        public int Height { get; }
        public int RefreshRate { get; }

        public bool IsValid => Width > 0
                               && Height > 0
                               && RefreshRate > 0;

        internal DisplayMode(int width, int height, int refreshRate)
        {
            Width = width;
            Height = height;
            RefreshRate = refreshRate;
        }

        internal SDL2.SDL_DisplayMode ToSdlDisplayMode()
        {
            var displayMode = new SDL2.SDL_DisplayMode
            {
                w = Width,
                h = Height,
                refresh_rate = RefreshRate
            };

            return displayMode;
        }

        public override string ToString()
        {
            var sb = new StringBuilder();

            sb.Append(Width);
            sb.Append('x');
            sb.Append(Height);
            sb.Append('@');
            sb.Append(RefreshRate);
            sb.Append("Hz");

            return sb.ToString();
        }
    }
}