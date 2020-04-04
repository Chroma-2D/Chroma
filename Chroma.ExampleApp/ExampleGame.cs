using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using Chroma.Diagnostics;
using Chroma.Graphics;
using Chroma.Graphics.TextRendering;

namespace Chroma.ExampleApp
{
    public class ExampleGame : Game
    {
        private TrueTypeFont _ttf;
        private float _rotation;
        private List<Color> _colors;

        public ExampleGame()
        {
            Graphics.VSyncEnabled = true;
            Log.Verbosity |= Verbosity.Debug;

            Window.GoWindowed(1024, 600);

            var loc = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);

            _colors = new List<Color>
            {
                Color.Red,
                Color.Orange,
                Color.Yellow,
                Color.Lime,
                Color.CornflowerBlue,
                Color.Indigo,
                Color.Violet
            };
            _ttf = new TrueTypeFont(Path.Combine(loc, "c64style.ttf"), 16);
        }
        
        protected override void Update(float delta)
        {
            Window.Properties.Title = $"{Window.FPS}";
            _rotation += 10f * delta;
        }

        protected override void Draw(RenderContext context)
        {
            context.Clear(Color.Black);

            context.DrawString(
                _ttf,
                "EAT YOUR BROCCOLI",
                new Vector2(16, 16),
                (c, i, p, g) =>
                {
                    var glyphTransform = new GlyphTransformData(p)
                    {
                        Color = _colors[i % _colors.Count]
                    };

                    var verticalNudge = 2 * (float)Math.Sin(i + _rotation);
                    var horizontalNudge = 3 * (float)Math.Sin(i + _rotation);

                    glyphTransform.Position = new Vector2(p.X + horizontalNudge, p.Y + verticalNudge);
                    return glyphTransform;
                }
            );
        }
    }
}