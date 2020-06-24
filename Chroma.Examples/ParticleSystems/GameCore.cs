using System;
using System.Globalization;
using System.IO;
using System.Numerics;
using Chroma;
using Chroma.ContentManagement.FileSystem;
using Chroma.Graphics;
using Chroma.Graphics.Particles;
using Chroma.Input;
using Chroma.Input.EventArgs;

namespace ParticleSystems
{
    public class GameCore : Game
    {
        private Texture _particle;
        private ParticleEmitter _emitter;

        public GameCore()
        {
            Content = new FileSystemContentProvider(this, Path.Combine(LocationOnDisk, "../../../../_common"));
        }
        
        // Please see Chroma/Graphics/Particles/StateInitializers/RandomizedStateInitializer
        // for how to implement a working particle state initializer.
        //
        // You can assign a new particle state initializer while instantiating
        // the particle emitter.
        //
        protected override void LoadContent()
        {
            _particle = Content.Load<Texture>("Textures/part.png");
            _particle.FilteringMode = TextureFilteringMode.NearestNeighbor;
            _emitter = new ParticleEmitter(_particle);
            _emitter.Density = 500;
            _emitter.EmissionRate = 10;
            
            _emitter.RegisterIntegrator(BuiltInParticleStateIntegrators.ScaleDown);
            _emitter.RegisterIntegrator(BuiltInParticleStateIntegrators.FadeOut);
            _emitter.RegisterIntegrator(CustomStateIntegrator);
        }

        protected override void Draw(RenderContext context)
        {
            _emitter.Draw(context);
            context.DrawString(
                "Move mouse around the window to change particle spawn position.\n" +
                "Press <LMB> to activate the particle emitter.",
                new Vector2(8)
            );
        }

        protected override void Update(float delta)
        {
            Window.Properties.Title = $"{Window.FPS} | {_emitter.Particles.Count} particles shown";
            
            _emitter.IsActive = Mouse.IsButtonDown(MouseButton.Left);
            _emitter.SpawnPosition = Mouse.GetPosition();

            _emitter.Update(delta);
        }

        private void CustomStateIntegrator(Particle part, float delta)
        {
            part.Position.X += part.Velocity.X * 5 * delta;
            part.Position.Y += part.Velocity.Y * MathF.Sin(Window.Properties.Width / part.Position.X) * delta;

            part.Velocity.X *=  (float)part.TTL / part.InitialTTL;
        }
    }
}