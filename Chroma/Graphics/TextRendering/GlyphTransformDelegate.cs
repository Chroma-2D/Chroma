using System.Numerics;
using Chroma.Graphics.TextRendering.Bitmap;
using Chroma.Graphics.TextRendering.TrueType;

namespace Chroma.Graphics.TextRendering
{
    public delegate void GlyphTransform(GlyphTransformData d, char c, int i, Vector2 p);
}