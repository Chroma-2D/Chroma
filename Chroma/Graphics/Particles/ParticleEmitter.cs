namespace Chroma.Graphics.Particles;

using System;
using System.Collections.Generic;
using System.Numerics;
using Chroma.Graphics.Particles.StateInitializers;

public class ParticleEmitter
{
    public delegate void ParticleStateIntegrator(Particle particle, float deltaTime);

    private readonly List<Particle> _particles;
    private readonly List<ParticleStateIntegrator> _stateIntegrators;

    public IReadOnlyList<Particle> Particles => _particles;
    public IReadOnlyList<ParticleStateIntegrator> StateIntegrators => _stateIntegrators;

    public int Density { get; set; } = 60;
    public int MaxParticleTTL { get; set; } = 120;

    public Texture Texture { get; set; }
    public ParticleStateInitializer ParticleStateInitializer { get; set; }

    public ParticleEmitter(Texture texture, ParticleStateInitializer initializer = null)
    {
        if (texture.Disposed)
            throw new ArgumentException("Texture provided was already disposed.", nameof(texture));

        Texture = texture;
        Texture.UseBlending = true;
        Texture.SetBlendingMode(BlendingPreset.NormalAddAlpha);

        _stateIntegrators = new List<ParticleStateIntegrator>();
        _particles = new List<Particle>(1200);

        ParticleStateInitializer = initializer ?? new RandomizedStateInitializer(this);
    }

    public void Emit(Vector2 initialPosition, int count)
    {
        if (Particles.Count >= Density)
            return;

        for (var i = 0; i < count; i++)
            CreateParticle(initialPosition);
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

    protected virtual void CreateParticle(Vector2 initialPosition)
    {
        _particles.Add(
            ParticleStateInitializer.CreateParticle(initialPosition)
        );
    }
}