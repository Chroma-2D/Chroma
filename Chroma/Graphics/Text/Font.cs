using System;
using Chroma.SDL2;

namespace Chroma.Graphics.Text
{
    public class Font
    {
        internal IntPtr Handle { get; }

        public GlyphAtlas Atlas { get; }
        public int Size { get; }

        public Font(string filePath, int size)
        {
            Size = size;
            Handle = SDL_ttf.TTF_OpenFont(filePath, Size);
            Atlas = new GlyphAtlas(this);
        }

        public bool HasGlyph(char c)
            => Atlas.GlyphMetadata.ContainsKey(c);

        public Vector2 Measure(string text)
        {
            SDL_ttf.TTF_SizeUNICODE(Handle, text, out int w, out int h);
            return new Vector2(w, h);
        }
    }
}
