using System.Numerics;

namespace Chroma.Graphics.Particles
{
    public class Particle
    {
        public int InitialTTL { get; }
        public float InitialRotation { get; }
        public Color InitialColor { get; }
        public Vector2 InitialScale { get; }
        public Vector2 InitialOrigin { get; }
        public Vector2 InitialPosition { get; }
        public Vector2 InitialVelocity { get; }

        public int TTL { get; internal set; }

        public float Rotation { get; set; }
        public Color Color { get; set; }

        public Vector2 Scale { get; set; }
        public Vector2 Origin { get; set; }
        public Vector2 Position { get; set; }
        public Vector2 Velocity { get; set; }

        public Particle(int ttl, float rotation, Color color, Vector2 scale, Vector2 origin, Vector2 position,
            Vector2 velocity)
        {
            TTL = InitialTTL = ttl;
            Rotation = InitialRotation = rotation;
            Color = InitialColor = color;
            Scale = InitialScale = scale;
            Origin = InitialOrigin = origin;
            Position = InitialPosition = position;
            Velocity = InitialVelocity = velocity;
        }
    }
}