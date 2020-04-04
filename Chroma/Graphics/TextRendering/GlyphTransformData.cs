namespace Chroma.Graphics.TextRendering
{
    public struct GlyphTransformData
    {
        public Vector2 Position { get; set; }
        public Vector2 Scale { get; set; }
        public Color Color { get; set; }
        public float Rotation { get; set; }

        public GlyphTransformData(Vector2 position)
        {
            Position = position;

            Scale = Vector2.One;
            Color = Color.White;
            Rotation = 0f;
        }
    }
}