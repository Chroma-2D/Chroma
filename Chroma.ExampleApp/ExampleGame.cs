using System;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using Chroma.Diagnostics;
using Chroma.Graphics;
using Chroma.Input;
using Chroma.Input.EventArgs;
using Chroma.Windowing;

namespace Chroma.ExampleApp
{
    public class ExampleGame : Game
    {
        private Texture _tex;
        private Stopwatch _sw;

        public ExampleGame()
        {
            _sw = new Stopwatch();
            Graphics.VSyncEnabled = false;
            Log.Verbosity |= Verbosity.Debug;

            Window.GoWindowed(1024, 600);

            var loc = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
            _tex = new Texture(Path.Combine(loc, "dvd.png"))
            {
                ColorMask = Color.White
            };
        }
        
        protected override void Update(float delta)
        {
            Window.Properties.Title = $"{Window.FPS}";
        }
        
        protected override void Draw(RenderContext context)
        {
            context.Clear(Color.CornflowerBlue);

            for (var x = 0; x < 100; x++)
            {
                for (var y = 0; y < 100; y++)
                {
                    context.DrawTexture(
                        _tex,
                        x * _tex.Width,
                        y * _tex.Height
                    );
                }
            }
        }
    }
}