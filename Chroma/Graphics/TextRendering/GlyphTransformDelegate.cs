using System.Numerics;
using Chroma.Graphics.TextRendering.Bitmap;
using Chroma.Graphics.TextRendering.TrueType;

namespace Chroma.Graphics.TextRendering
{
    public delegate GlyphTransformData GlyphTransform(char c, int i, Vector2 p);
}