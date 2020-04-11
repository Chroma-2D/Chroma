using System.Numerics;

namespace Chroma.Graphics
{
    public struct Vertex
    {
        public Vector2 Position { get; }
        public Color Color { get; }

        public Vertex(Vector2 position, Color color)
        {
            Position = position;
            Color = color;
        }

        public Vertex(Vector2 position)
            : this(position, Color.White) { }

        public Vertex(float x, float y, Color color)
            : this(new Vector2(x, y), color) { }

        public Vertex(float x, float y)
            : this(new Vector2(x, y)) { }

        internal float[] ToGpuVertexArray()
            => new float[] { Position.X, Position.Y };
    }
}
