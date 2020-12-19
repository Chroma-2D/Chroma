using System.IO;
using System.Numerics;
using Chroma;
using Chroma.ContentManagement.FileSystem;
using Chroma.Diagnostics;
using Chroma.Graphics;
using Chroma.Graphics.Particles;
using Chroma.Input;

namespace ParticleSystems
{
    public class GameCore : Game
    {
        private Texture _particle;
        private RenderTarget _target;
        private ParticleEmitter _emitter;

        public GameCore()
        {
            Content = new FileSystemContentProvider(Path.Combine(LocationOnDisk, "../../../../_common"));
        }
        
        // Please see Chroma/Graphics/Particles/StateInitializers/RandomizedStateInitializer
        // for how to implement a working particle state initializer.
        //
        // You can assign a new particle state initializer while instantiating
        // the particle emitter.
        //
        protected override void LoadContent()
        {
            _target = new RenderTarget(Window.Size.Width, Window.Size.Height);
            
            _particle = Content.Load<Texture>("Textures/pentagram.png");
            _particle.FilteringMode = TextureFilteringMode.NearestNeighbor;
            _emitter = new ParticleEmitter(_particle);
            _emitter.Density = 300;
            _emitter.EmissionRate = 10;
            
            _emitter.RegisterIntegrator(BuiltInParticleStateIntegrators.ScaleDown);
            _emitter.RegisterIntegrator(BuiltInParticleStateIntegrators.FadeOut);
            _emitter.RegisterIntegrator(CustomStateIntegrator);
        }

        protected override void Draw(RenderContext context)
        {
            context.RenderTo(_target, () =>
            {
                context.Clear(Color.Black);
                _emitter.Draw(context);
            });
            
            context.DrawTexture(_target, Vector2.Zero, Vector2.One, Vector2.Zero, 0);
            
            context.DrawString(
                "Move mouse around the window to change particle spawn position.\n" +
                "Press <LMB> to activate the particle emitter.",
                new Vector2(8)
            );
        }

        protected override void Update(float delta)
        {
            Window.Title = $"{PerformanceCounter.FPS} | {_emitter.Particles.Count} particles shown";
            
            _emitter.IsActive = Mouse.IsButtonDown(MouseButton.Left);
            _emitter.SpawnPosition = Mouse.GetPosition();

            _emitter.Update(delta);
        }

        private void CustomStateIntegrator(Particle part, float delta)
        {
            part.Origin = part.Owner.Texture.Center;
            
            part.Position.X += part.Velocity.X * 5 * delta;
            part.Position.Y += part.Velocity.Y * delta;

            part.Rotation += part.Velocity.X - part.Velocity.Y * delta;

            part.Velocity.X *=  (float)part.TTL / part.InitialTTL;
            if (part.Velocity.Y < 0)
                part.Velocity.Y *= -1;
        }
    }
}
