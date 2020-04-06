using System.IO;
using System.Reflection;
using Chroma.Diagnostics;
using Chroma.Graphics;

namespace Chroma.ExampleApp
{
    public class ExampleGame : Game
    {
        private Texture _tex;
        private Texture _wall1;
        
        public ExampleGame()
        {
            Graphics.VSyncEnabled = false;
            Log.Verbosity |= Verbosity.Debug;

            Window.GoWindowed(1024, 600);

            var loc = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
            _tex = new Texture(Path.Combine(loc, "walls.jpeg"));

            _wall1 = new Texture(64, 64);

            for(var y = 0; y < 64; y++)
            {
                for(var x = 0; x < 64; x++)
                {
                    _wall1[x, y] = _tex[x, y];
                }
            }
            _wall1.Flush();
        }

        protected override void Update(float delta)
        {
            Window.Properties.Title = $"{Window.FPS}";
        }

        protected override void Draw(RenderContext context)
        {
            context.DrawTexture(_wall1, Vector2.Zero, Vector2.One, Vector2.Zero, .0f);
        }
    }
}