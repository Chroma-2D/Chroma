namespace Chroma.Graphics.Particles;

using System.Numerics;

public abstract class ParticleStateInitializer
{
    protected ParticleEmitter Emitter { get; }

    protected ParticleStateInitializer(ParticleEmitter emitter)
        => Emitter = emitter;

    public abstract Particle CreateParticle(Vector2 initialPosition);
}