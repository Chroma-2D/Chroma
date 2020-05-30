using System;
using System.Collections.Generic;
using System.Numerics;
using Chroma.Graphics.Particles.StateInitializers;

namespace Chroma.Graphics.Particles
{
    public class ParticleEmitter
    {
        public delegate void ParticleStateIntegrator(Particle particle, float deltaTime);

        public int EmissionRate { get; set; } = 1;

        private List<Particle> _particles;
        private List<ParticleStateIntegrator> _stateIntegrators;

        public IReadOnlyList<Particle> Particles => _particles;
        public IReadOnlyList<ParticleStateIntegrator> StateIntegrators => _stateIntegrators;
        public ParticleStateInitializer ParticleStateInitializer { get; set; }

        public int Density { get; set; } = 60;
        public int MaxParticleTTL { get; set; } = 120;

        public Vector2 SpawnPosition { get; set; }

        public bool IsActive { get; set; }
        public Texture Texture { get; set; }

        public ParticleEmitter(Texture texture)
        {
            if (texture.Disposed)
                throw new ArgumentException("Texture provided was already disposed.", nameof(texture));

            Texture = texture;
            Texture.UseBlending = true;
            Texture.SetBlendingMode(BlendingPreset.NormalAddAlpha);

            _stateIntegrators = new List<ParticleStateIntegrator>();
            _particles = new List<Particle>();
            
            ParticleStateInitializer = new RandomizedStateInitializer(this);
            RegisterIntegrator(BuiltInParticleStateIntegrators.LinearStateIntegrator);
        }

        public void RegisterIntegrator(ParticleStateIntegrator integrator)
        {
            if (integrator == null)
                throw new ArgumentNullException(nameof(integrator), "Cannot register a null integrator.");

            if (!_stateIntegrators.Contains(integrator))
                _stateIntegrators.Add(integrator);
        }

        public void UnregisterIntegrator(ParticleStateIntegrator integrator)
        {
            if (integrator == null)
                throw new ArgumentNullException(nameof(integrator), "Cannot unregister a null intergrator.");

            if (_stateIntegrators.Contains(integrator))
                _stateIntegrators.Remove(integrator);
        }

        public void UnregisterAllIntegrators()
            => _stateIntegrators.Clear();

        public virtual void Update(float delta)
        {
            for (var i = 0; i < Particles.Count; i++)
            {
                var part = Particles[i];

                foreach (var integrator in StateIntegrators)
                    integrator(part, delta);

                part.TTL--;

                if (part.TTL <= 0)
                    _particles.RemoveAt(i);
            }

            if (IsActive)
            {
                if (Particles.Count < Density)
                {
                    for (var limit = 0; limit < EmissionRate; limit++)
                        CreateParticle();
                }
            }
        }

        public virtual void Draw(RenderContext context)
        {
            for (var i = 0; i < Particles.Count; i++)
            {
                var part = Particles[i];

                Texture.ColorMask = part.Color;

                if (part.TextureSourceRectangle.IsEmpty)
                {
                    context.DrawTexture(
                        Texture,
                        part.Position,
                        part.Scale,
                        part.Origin,
                        part.Rotation
                    );
                }
                else
                {
                    context.DrawTexture(
                        Texture,
                        part.Position,
                        part.Scale,
                        part.Origin,
                        part.Rotation,
                        part.TextureSourceRectangle
                    );
                }

                Texture.ColorMask = Color.White;
            }
        }

        protected virtual void CreateParticle()
        {
            _particles.Add(ParticleStateInitializer.Provide());
        }
    }
}