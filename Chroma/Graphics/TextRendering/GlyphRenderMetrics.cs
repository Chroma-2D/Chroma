using System.Drawing;
using System.Numerics;

namespace Chroma.Graphics.TextRendering
{
    public struct GlyphRenderMetrics
    {
        public Rectangle Bounds { get; }
        public Vector2 RenderOffsets { get; }
        public int HorizontalAdvance { get; }

        internal GlyphRenderMetrics(Rectangle bounds, Vector2 renderOffsets, int horizontalAdvance)
        {
            Bounds = bounds;
            RenderOffsets = renderOffsets;
            HorizontalAdvance = horizontalAdvance;
        }
    }
}