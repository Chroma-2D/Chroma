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
        private Texture _tex;
        private RenderTarget _tgt;

        private Stopwatch _sw;
        private TrueTypeFont _ttf;

        private Vector2 _origin;
        private float _rotation;
        private byte _alpha;
        private List<Color> _colors;

        public ExampleGame()
        {
            _sw = new Stopwatch();
            Graphics.VSyncEnabled = false;
            Log.Verbosity |= Verbosity.Debug;

            Window.GoWindowed(1024, 600);

            var loc = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);

            _tgt = new RenderTarget((ushort)Window.Properties.Width, (ushort)Window.Properties.Height);

            _tex = new Texture(Path.Combine(loc, "delet.png"))
            {
                ColorMask = Color.White,
            };

            _origin = new Vector2(
                _tex.Width * .3f / 2,
                _tex.Height * .3f / 2
            );

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
            _ttf = new TrueTypeFont(Path.Combine(loc, "DooM.ttf"), 16);
        }

        protected override void Update(float delta)
        {
            Window.Properties.Title = $"{Window.FPS}";
            _rotation += 10f * delta;

            _alpha = 255;
            _tex.ColorMask = new Color(255, 255, 255, _alpha);
        }

        protected override void Draw(RenderContext context)
        {
            context.Clear(Color.Black);

            context.DrawString(
                _ttf,
                "PICKED UP SOME SHELLS",
                new Vector2(16),
                (c, i, p, g) =>
                {
                    var glyphTransform = new GlyphTransformData(p)
                    {
                        Color = Color.Red
                    };

                    var verticalNudge = 2 * (float)Math.Sin(i + _rotation);

                    glyphTransform.Position = new Vector2(p.X, p.Y + verticalNudge);

                    return glyphTransform;
                }
            );

            context.DrawString(
                _ttf,
                "PICKED UP BFG AMMO!",
                new Vector2(16, 32),
                (c, i, p, g) =>
                {
                    var glyphTransform = new GlyphTransformData(p)
                    {
                        Color = Color.Red
                    };

                    if (c == 'B' || c == 'F' || c == 'G')
                        glyphTransform.Color = Color.Lime;

                    var horizontalNudge = 3 * (float)Math.Cos(i + _rotation);

                    glyphTransform.Position = new Vector2(p.X + horizontalNudge, p.Y);

                    return glyphTransform;
                }
            );

            /*context.DrawTexture(
                 _ttf.Atlas,
                 Vector2.Zero,//new Vector2(300, 300), 
                 Vector2.One,
                 Vector2.Zero, .0f
             );*/
        }
    }
}