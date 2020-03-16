using Chroma.Graphics;
using System;
using System.Collections.Generic;

namespace Chroma.ExampleApp
{
    public class ExampleGame : Game
    {
        private readonly List<Vertex> _verts = new List<Vertex>();

        public ExampleGame()
        {
            // Window.SwitchToExclusiveFullscreen();
            Window.SwitchToWindowed(1024, 600);
            Window.VSyncEnabled = true;

            _verts.AddRange(
                new[]
                {
                    new Vertex(32f, 32f),
                    new Vertex(64f, 32f),
                    new Vertex(64f, 64f),
                    new Vertex(32f, 64f),
                });
        }

        protected override void Draw(RenderContext context)
        {
            context.Clear(Color.Black);
            context.Line(new Vector2(32, 32), new Vector2(64, 64), Color.White);
        }

        protected override void Update(float delta)
        {
            Window.Title = Window.FPS.ToString();
        }
    }
}
