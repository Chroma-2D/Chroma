using System.Drawing;
using System.Numerics;

namespace Chroma.Graphics.Particles
{
    public class Particle
    {
        public ParticleEmitter Owner { get; }
        
        public int InitialTTL { get; }
        public float InitialRotation { get; }
        public Color InitialColor { get; }
        public Vector2 InitialScale { get; }
        public Vector2 InitialOrigin { get; }
        public Vector2 InitialPosition { get; }
        public Vector2 InitialVelocity { get; }

        public int TTL { get; internal set; }

        public float Rotation;
        public Color Color;

        public Vector2 Scale;
        public Vector2 Origin;
        public Vector2 Position;
        public Vector2 Velocity;

        public Rectangle TextureSourceRectangle;

        public Particle(ParticleEmitter owner, int ttl, float rotation, Color color, Vector2 scale, Vector2 origin, Vector2 position,
            Vector2 velocity, Rectangle textureSourceRectangle)
        {
            Owner = owner;
            TTL = InitialTTL = ttl;
            Rotation = InitialRotation = rotation;
            Color = InitialColor = color;
            Scale = InitialScale = scale;
            Origin = InitialOrigin = origin;
            Position = InitialPosition = position;
            Velocity = InitialVelocity = velocity;
            TextureSourceRectangle = textureSourceRectangle;
        }
    }
}