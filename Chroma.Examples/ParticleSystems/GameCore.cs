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

            Graphics.LimitFramerate = false;
            Graphics.VSyncEnabled = false;
        }
        
        protected override void LoadContent()
        {
            _particle = Content.Load<Texture>("Textures/part.png");
            _particle.FilteringMode = TextureFilteringMode.NearestNeighbor;
            _emitter = new ParticleEmitter(_particle);
            _emitter.Density = 20000;
            _emitter.EmissionRate = 500;
            
            _emitter.RegisterIntegrator(BuiltInParticleStateIntegrators.LinearPositionX);
            _emitter.RegisterIntegrator(BuiltInParticleStateIntegrators.LinearPositionY);
            _emitter.RegisterIntegrator(BuiltInParticleStateIntegrators.ScaleDown);
            _emitter.RegisterIntegrator(BuiltInParticleStateIntegrators.FadeOut);
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
    }
}