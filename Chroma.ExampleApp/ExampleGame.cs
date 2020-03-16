using Chroma.Graphics;
using System;
using System.Collections.Generic;

namespace Chroma.ExampleApp
{
    public class ExampleGame : Game
    {
        private List<Vertex> _verts = new List<Vertex>();

        public ExampleGame()
        {
            Window.SwitchToWindowed(1024, 600);

            _verts.AddRange(
                new[]
                {
                    new Vertex(32f, 32f),
                    new Vertex(64f, 32f),
                    new Vertex(64f, 64f),
                    new Vertex(32f, 64f)
                });
        }

        protected override void Draw(RenderContext context)
        {
            context.Clear(Color.Black);

            context.LineThickness = 2;
            context.Polygon(ShapeMode.Stroke, _verts, Color.White);
        }

        protected override void Update(float delta)
        {
            Console.WriteLine(Window.FPS);
        }
    }
}
