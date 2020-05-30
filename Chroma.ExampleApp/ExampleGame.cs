using System.Drawing;
using System.IO;
using System.Numerics;
using Chroma.Graphics;
using Chroma.Graphics.Particles;
using Chroma.Graphics.Particles.StateInitializers;
using Chroma.Graphics.TextRendering;
using Chroma.Input;
using Chroma.Input.EventArgs;
using Color = Chroma.Graphics.Color;

namespace Chroma.ExampleApp
{
    public class RectangleSourceInitializer : RandomizedStateInitializer
    {
        public Rectangle SourceRectangle;
        
        public RectangleSourceInitializer(ParticleEmitter emitter) 
            : base(emitter)
        {
        }

        public override Particle Provide()
        {
            var part = base.Provide();
    
            part.TextureSourceRectangle = SourceRectangle;
            part.Origin = new Vector2(SourceRectangle.Width / 2, SourceRectangle.Height / 2);
            
            return part;
        }
    }

    public class ExampleGame : Game
    {
        private ParticleEmitter _emitter;
        private ImageFont _imf;
        private RenderTarget _tgt;
        private RectangleSourceInitializer _rsi;
        private string _str;

        public ExampleGame()
        {
            Graphics.VSyncEnabled = false;

            Window.GoWindowed(1024, 640);
            Graphics.AutoClearColor = Color.CornflowerBlue;
        }

        protected override void LoadContent()
        {
            _tgt = new RenderTarget(512, 512);
            _tgt.FilteringMode = TextureFilteringMode.NearestNeighbor;
            
            _emitter = new ParticleEmitter(_tgt);
            _rsi = new RectangleSourceInitializer(_emitter);
            
            _emitter.MaxParticleTTL = 1800;
            _emitter.Density = 15;
            _emitter.ParticleStateInitializer = _rsi;
            
            _imf = Content.Load<ImageFont>("ImageFonts/DialogFont.png", "ABCDEFGHIJKLMNOPQRSTUVWXYZ1234567890 ");
        }

        protected override void Update(float delta)
        {
            _str = $"{Window.FPS}";

            var m = _imf.Measure(_str);
            _rsi.SourceRectangle = new Rectangle(0, 0, (int)m.X, (int)m.Y);
            
            _emitter.Update(delta);

            Window.Properties.Title = delta.ToString();
        }

        protected override void Draw(RenderContext context)
        {
            context.RenderTo(_tgt, () =>
            {
                context.Clear(Color.Transparent);
                context.DrawString(_imf, _str, new Vector2(0));
            });

            _emitter.Draw(context);
        }

        protected override void KeyPressed(KeyEventArgs e)
        {
            if (e.KeyCode == KeyCode.Return)
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