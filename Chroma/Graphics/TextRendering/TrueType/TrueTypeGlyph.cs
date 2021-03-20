using System.Numerics;

namespace Chroma.Graphics.TextRendering.TrueType
{
    internal struct TrueTypeGlyph
    {
        public Vector2 Position { get; internal set; }
        public Vector2 Size { get; internal set; }
        public Vector2 Bearing { get; internal set; }
        public Vector2 Advance { get; internal set; }
    }
}