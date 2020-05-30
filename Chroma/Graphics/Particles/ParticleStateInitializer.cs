namespace Chroma.Graphics.Particles
{
    public abstract class ParticleStateInitializer
    {
        protected ParticleEmitter Emitter { get; }

        public ParticleStateInitializer(ParticleEmitter emitter)
        {
            Emitter = emitter;
        }

        public abstract Particle Provide();
    }
}