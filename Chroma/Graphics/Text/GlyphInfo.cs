namespace Chroma.Graphics.Text
{
    public struct GlyphInfo
    {
        public readonly int MinOffsetX;
        public readonly int MaxOffsetX;
        public readonly int MinOffsetY;
        public readonly int MaxOffsetY;
        public readonly int Advance;

        public readonly int Width;
        public readonly int Height;
        public readonly Vector2 PositionInAtlas;

        public GlyphInfo(int minX, int maxX, int minY, int maxY, int adv, int width, int height, Vector2 posInAtlas)
        {
            MinOffsetX = minX;
            MaxOffsetX = maxX;
            MinOffsetY = minY;
            MaxOffsetY = maxY;
            Advance = adv;

            Width = width;
            Height = height;
            PositionInAtlas = posInAtlas;
        }
    }
}
