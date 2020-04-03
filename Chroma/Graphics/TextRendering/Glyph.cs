namespace Chroma.Graphics.TextRendering
{
    public struct Glyph
    {
        public Vector2 Position { get; internal set; }
        public Vector2 Dimensions { get; internal set; }

        public Vector2 Offset { get; internal set; }
        public int Advance { get; internal set; }
    }
}
