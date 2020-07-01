using System;
using System.Drawing;
using System.Numerics;

namespace Chroma.Graphics.Particles.StateInitializers
{
    public class RandomizedStateInitializer : ParticleStateInitializer
    {
        private Random Random { get; set; }
        
        public Vector2 VelocityLimits { get; set; } = new Vector2(200, 400);
        
        public Vector2 InitialVelocity => new Vector2(
            Random.Next((int)VelocityLimits.X) * (Random.Next(-1, 1) == -1 ? -1 : 1),
            Random.Next((int)VelocityLimits.Y) * (Random.Next(-1, 1) == -1 ? -1 : 1)
        );
        
        public virtual Vector2 InitialPosition => Emitter.SpawnPosition;
        public virtual Vector2 InitialOrigin => new Vector2(Emitter.Texture.Width / 2, Emitter.Texture.Height / 2);
        public virtual Vector2 InitialScale => Vector2.One * Random.Next(1, 2);
        
        public virtual Color InitialColor => Color.White;
        public virtual float InitialRotation => 0;
        public virtual int InitialTTL => Random.Next(Emitter.MaxParticleTTL);

        public RandomizedStateInitializer(ParticleEmitter emitter)
            : base(emitter)
        {
            Random = new Random();
        }

        public override Particle CreateParticle()
        {
            return new Particle(
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