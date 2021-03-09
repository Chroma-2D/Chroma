using System.Collections.Generic;
using System.Drawing;

namespace Chroma.Graphics.TextRendering
{
    public interface IFontProvider<T>
    {
        string FileName { get; }

        int Height { get; }
        int LineSpacing { get; }
        bool UseKerning { get; set; }
        Dictionary<char, T> Glyphs { get; }

        bool HasGlyph(char c);
        Size Measure(string s);

        Texture GetTexture(char c = (char)0);
    }
}