using System.Numerics;

namespace Chroma.Graphics.Particles
{
    public abstract class ParticleStateInitializer
    {
        protected ParticleEmitter Emitter { get; }

        protected ParticleStateInitializer(ParticleEmitter emitter)
            => Emitter = emitter;

        public abstract Particle CreateParticle(Vector2 initialPosition);
    }
}