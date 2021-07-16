using System;
using Chroma;
using Chroma.Diagnostics.Logging;
using Chroma.Graphics;

namespace EmptyProject
{
    internal class GameCore : Game
    {
        private Log Log { get; } = LogManager.GetForCurrentAssembly();

        internal GameCore() : base(new(false, false))
        {
            Log.Info("Hello, world!");
            Window.GoWindowed(320, 240);

            foreach (var ext in Graphics.GlExtensions)
            {
                Console.WriteLine(ext);
            }
        }

        protected override void Draw(RenderContext context)
        {
            // context.Pixel(new(0f, 0f), Color.Aqua);
            //
            // context.Line(0, 2, 0, 50, Color.Aqua);
            //
            // context.Line(2, 0, 50, 0, Color.Aqua);
            
            context.Rectangle(
                ShapeMode.Stroke,
                Window.Size.Width - 1 - 8,
                Window.Size.Height - 1 - 8,
                8,
                8,
                Color.Red
            );
            //
            // context.Rectangle(
            //     ShapeMode.Stroke,
            //     0, 0, 8, 8, Color.Aqua);
            //     
            
            context.Triangle(
                ShapeMode.Fill,
                new(280, 0),
                new(319, 0),
                new(319, 80),
                Color.Aqua
            );
        }
    }
}