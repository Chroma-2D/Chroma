using System.Numerics;
using Chroma.Graphics.TextRendering.Bitmap;
using Chroma.Graphics.TextRendering.TrueType;

namespace Chroma.Graphics.TextRendering
{
    public delegate GlyphTransformData GlyphTransform(char c, int i, Vector2 p);
    public delegate GlyphTransformData BitmapGlyphTransform(char c, int i, Vector2 p, BitmapGlyph g);
    public delegate GlyphTransformData TrueTypeFontGlyphTransform(char c, int i, Vector2 p, TrueTypeGlyph g);
}