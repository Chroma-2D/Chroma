using System.IO;
using System.Numerics;
using Chroma.Graphics;
using Chroma.Graphics.Particles;
using Chroma.Graphics.TextRendering;
using Chroma.Input;
using Chroma.Input.EventArgs;

namespace Chroma.ExampleApp
{
    public class ExampleGame : Game
    {
        private SpriteSheet _ss;
        private SpriteSheetAnimation _walkRight;
        private Texture _burg;
        private ParticleEmitter _emitter;
        private ImageFont _imf;
        
        public ExampleGame()
        {
            Graphics.VSyncEnabled = false;

            Window.GoWindowed(1024, 640);
            Graphics.AutoClearColor = Color.CornflowerBlue;
        }

        protected override void LoadContent()
        {
            _burg = Content.Load<Texture>("Textures/cate.png");
            
            _emitter = new ParticleEmitter(_burg);
            _emitter.MaxParticleTTL = 2000;
            _emitter.Density = 200;
            
            _ss = new SpriteSheet(
                Path.Combine(LocationOnDisk, "Content/Animations/skelly.png"),
                64, 64
            );

            _walkRight = new SpriteSheetAnimation(_ss, 27, 35, 120);
            _walkRight.Repeat = true;
            _ss.Position = new Vector2(30f);

            _imf = Content.Load<ImageFont>("ImageFonts/DialogFont.png", "ABCDEFGHIJKLMNOPQRSTUVWXYZ1234567890 ");
        }

        protected override void Update(float delta)
        {
            _emitter.Update(delta);
            Window.Properties.Title = delta.ToString();
            _walkRight.Update(delta);
        }

        protected override void Draw(RenderContext context)
        {
            _emitter.Draw(context);
            _walkRight.Draw(context);
            context.DrawString(_imf, $"FPS: {Window.FPS}\nPARTICLES: {_emitter.Particles.Count}", new Vector2(8));
        }

        protected override void KeyPressed(KeyEventArgs e)
        {
            if (e.KeyCode == KeyCode.Space)
            {
                _walkRight.Play();
            }
            else if (e.KeyCode == KeyCode.Return)
            {
                _emitter.IsActive = !_emitter.IsActive;
            }
        }

        protected override void MouseMoved(MouseMoveEventArgs e)
        {
            _emitter.SpawnPosition = e.Position;
        }
    }
}