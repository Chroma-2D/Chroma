using System.Numerics;
using Chroma.Graphics.TextRendering;
using Chroma.Graphics.TextRendering.Bitmap;

namespace Chroma.Graphics
{
    public delegate GlyphTransformData GlyphTransform(char c, int i, Vector2 p);
    public delegate GlyphTransformData BitmapGlyphTransform(char c, int i, Vector2 p, BitmapGlyph g);
    public delegate GlyphTransformData TrueTypeFontGlyphTransform(char c, int i, Vector2 p, TrueTypeGlyph g);
}