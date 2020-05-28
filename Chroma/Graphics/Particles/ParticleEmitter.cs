using System;
using System.Collections.Generic;
using System.Drawing;
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
        public Rectangle SpawnAreaLimits { get; set; }
        
        public bool IsActive { get; set; }
        public Texture Texture { get; set; }

        public ParticleEmitter(Texture texture)
        {
            if (texture.Disposed)
                throw new ArgumentException("Texture provided was already disposed.", nameof(texture));

            Particles = new List<Particle>();
            
            Random = new Random();
            
            Texture = texture;
            Texture.UseBlending = true;
            Texture.SetBlendingMode(BlendingPreset.NormalAddAlpha);
        }

        public virtual void Update(float delta)
        {
            if (!IsActive)
                return;
            
            for (var i = 0; i < Particles.Count; i++)
            {
                var part = Particles[i];

                part.Opacity = part.TTL / (float)MaxParticleTTL;
                part.Position += part.Velocity * delta * part.Direction;

                part.TTL--;

                if (part.TTL <= 0)
                    Particles.RemoveAt(i);
            }
            
            if (Particles.Count < Density)
            {
                for (var limit = 0; limit < EmissionRate; limit++)
                    CreateParticle();
            }
        }

        public void Draw(RenderContext context)
        {
            if (!IsActive)
                return;
            
            for (var i = 0; i < Particles.Count; i++)
            {
                var part = Particles[i];

                Texture.ColorMask = new Color(
                    255,
                    255,
                    255,
                    (byte)(255 * part.Opacity)
                );

                context.DrawTexture(Texture, part.Position, part.Scale / 2, new Vector2(Texture.Width / 2, Texture.Height / 2), part.Rotation);

                Texture.ColorMask = Color.White;
            }
        }

        protected void CreateParticle()
        {
            var part = new Particle
            {
                Position = ProvidePosition(),
                Direction = ProvideDirection(),
                Opacity = 1.0f,
                Rotation = ProvideRotation(),
                Scale = Vector2.One,
                TTL = Random.Next(MaxParticleTTL),
                Velocity = ProvideVelocity()
            };

            Particles.Add(part);
        }

        protected virtual Vector2 ProvideVelocity()
            => new Vector2(
                Random.Next((int)VelocityLimits.X),
                Random.Next((int)VelocityLimits.Y)
            );

        protected virtual Vector2 ProvidePosition()
        {
            return SpawnPosition +
                   new Vector2(
                       Random.Next(SpawnAreaLimits.X, SpawnAreaLimits.Width),
                       Random.Next(SpawnAreaLimits.Y, SpawnAreaLimits.Height)
                   );
        }

        protected virtual Vector2 ProvideDirection()
        {
            return new Vector2(
                Random.Next(-1, 1) == -1 ? -1 : 1,
                Random.Next(-1, 1) == -1 ? -1 : 1
            );
        }

        protected virtual float ProvideRotation()
            => (float)(Random.NextDouble() * 360);
    }
}