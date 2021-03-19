using System.Collections.Generic;
using System.Drawing;
using System.Numerics;

namespace Chroma.Graphics.TextRendering
{
    public interface IFontProvider
    {
        string FamilyName { get; }

        int Height { get; }
        int LineSpacing { get; }
        
        bool UseKerning { get; set; }
        
        bool HasGlyph(char c);
        Size Measure(string s);

        int GetKerning(char left, char right);
        int GetHorizontalAdvance(char c);
        
        Texture GetTexture(char c = (char)0);
        Rectangle GetGlyphBounds(char c);
        Vector2 GetRenderOffsets(char c);
    }
    
    public interface IFontProvider<T> : IFontProvider
    {
        Dictionary<char, T> Glyphs { get; }
    }
}