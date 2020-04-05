using System;
using System.Collections.Generic;
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
        private Texture _tex;

        private float _rotation;
        private float _rotation2;

        private List<Color> _colors;

        public ExampleGame()
        {
            Graphics.VSyncEnabled = false;
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

            _tex = new Texture(Path.Combine(loc, "burg.png"));
            _ttf = new TrueTypeFont(Path.Combine(loc, "c64style.ttf"), 16);
        }

        protected override void Update(float delta)
        {
            Window.Properties.Title = $"{Window.FPS}";

            _rotation += 30f * delta;
            _rotation %= 360;

            _rotation2 += 100f * delta;
            _rotation %= 360;
        }

        protected override void Draw(RenderContext context)
        {
            context.DrawTexture(
                _tex,
                new Vector2(400, 72),
                Vector2.One,
                new Vector2(_tex.Width / 2, _tex.Height / 2), 
                _rotation2
            );

            context.DrawString(
                _ttf,
                "BURG EMOTE WHEN",
                new Vector2(300, 140),
                (c, i, p, g) =>
                {
                    var transform = new GlyphTransformData(p)
                    {
                        Color = _colors[i % _colors.Count]
                    };

                    var verticalNudge = 2f * (float)Math.Sin(i + _rotation);
                    var horizontalNudge = 3f * (float)Math.Cos(i + _rotation);

                    transform.Position = new Vector2(p.X + horizontalNudge, p.Y + verticalNudge);

                    return transform;
                }
            );
        }
    }
}