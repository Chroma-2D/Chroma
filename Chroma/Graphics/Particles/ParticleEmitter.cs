using System;
using System.Collections.Generic;
using System.Numerics;

namespace Chroma.Graphics.Particles
{
    public class ParticleEmitter
    {
        protected Random Random { get; set; }

        public int EmissionRate { get; set; } = 1;
        public List<Particle> Particles { get; }

        public int Density { get; set; } = 60;
        public int MaxParticleTTL { get; set; } = 120;

        public Vector2 VelocityLimits { get; set; } = new Vector2(200, 400);
        public Vector2 SpawnPosition { get; set; }

        public bool IsActive { get; set; }
        public Texture Texture { get; set; }

        public virtual Vector2 InitialVelocity => new Vector2(
            Random.Next((int)VelocityLimits.X) * (Random.Next(-1, 1) == -1 ? -1 : 1),
            Random.Next((int)VelocityLimits.Y) * (Random.Next(-1, 1) == -1 ? -1 : 1)
        );

        public virtual Vector2 InitialPosition => SpawnPosition;
        public virtual Vector2 InitialOrigin => new Vector2(Texture.Width / 2, Texture.Height / 2);
        public virtual Vector2 InitialScale => Vector2.One / 8;

        public virtual Color InitialColor => Color.White;
        public virtual float InitialRotation => (float)(Random.NextDouble() * 360);
        public virtual int InitialTTL => Random.Next(MaxParticleTTL);

        public ParticleEmitter(Texture texture)
        {
            if (texture.Disposed)
                throw new ArgumentException("Texture provided was already disposed.", nameof(texture));

            Texture = texture;
            Texture.UseBlending = true;
            Texture.SetBlendingMode(BlendingPreset.NormalAddAlpha);
            
            Particles = new List<Particle>();
            Random = new Random();
        }

        public void Update(float delta)
        {
            for (var i = 0; i < Particles.Count; i++)
            {
                var part = Particles[i];

                part.Velocity = ProvideVelocity(part);
                part.Position = ProvidePosition(part, delta);
                part.Color = ProvideColor(part);
                part.Scale = ProvideScale(part);
                part.Rotation = ProvideRotation(part);
                part.Origin = ProvideOrigin(part);

                part.TTL--;

                if (part.TTL <= 0)
                    Particles.RemoveAt(i);
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

        public void Draw(RenderContext context)
        {
            for (var i = 0; i < Particles.Count; i++)
            {
                var part = Particles[i];

                Texture.ColorMask = part.Color;

                context.DrawTexture(
                    Texture,
                    part.Position,
                    part.Scale,
                    part.Origin,
                    part.Rotation
                );

                Texture.ColorMask = Color.White;
            }
        }

        protected void CreateParticle()
        {
            Particles.Add(new Particle(
                InitialTTL,
                InitialRotation,
                InitialColor,
                InitialScale,
                InitialOrigin,
                InitialPosition,
                InitialVelocity
            ));
        }

        protected virtual Color ProvideColor(Particle particle)
        {
            var color = new Color(particle.Color);
            color.A = (byte)(255 * ((float)particle.TTL / particle.InitialTTL));

            return color;
        }

        protected virtual float ProvideRotation(Particle particle)
            => particle.Rotation;
        
        protected virtual Vector2 ProvideOrigin(Particle particle)
            => particle.Origin;

        protected virtual Vector2 ProvideVelocity(Particle particle)
            => particle.Velocity;

        protected virtual Vector2 ProvideScale(Particle particle)
            => particle.InitialScale * ((float)particle.TTL / particle.InitialTTL);

        protected virtual Vector2 ProvidePosition(Particle particle, float deltaTime)
            => particle.Position + (particle.Velocity * deltaTime);
    }
}