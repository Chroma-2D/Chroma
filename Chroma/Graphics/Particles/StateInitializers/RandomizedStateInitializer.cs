using System;
using System.Drawing;
using System.Numerics;

namespace Chroma.Graphics.Particles.StateInitializers
{
    public class RandomizedStateInitializer : ParticleStateInitializer
    {
        private Random Random { get; } = new();

        public Vector2 VelocityLimits { get; set; } = new(200, 400);

        public Vector2 InitialVelocity => new(
            Random.Next((int)VelocityLimits.X) * (Random.Next(-1, 1) == -1 ? -1 : 1),
            Random.Next((int)VelocityLimits.Y) * (Random.Next(-1, 1) == -1 ? -1 : 1)
        );

        public virtual Vector2 InitialPosition => Emitter.SpawnPosition;

        public virtual Vector2 InitialOrigin => new(
            Emitter.Texture.Width / 2f,
            Emitter.Texture.Height / 2f
        );

        public virtual Vector2 InitialScale => Vector2.One * Random.Next(1, 2);

        public virtual Color InitialColor => Color.White;
        public virtual float InitialRotation => 0;
        public virtual int InitialTTL => Random.Next(Emitter.MaxParticleTTL);

        public RandomizedStateInitializer(ParticleEmitter emitter)
            : base(emitter)
        {
        }

        public override Particle CreateParticle()
        {
            return new(
                Emitter,
                InitialTTL,
                InitialRotation,
                InitialColor,
                InitialScale,
                InitialOrigin,
                InitialPosition,
                InitialVelocity,
                Rectangle.Empty
            );
        }
    }
}