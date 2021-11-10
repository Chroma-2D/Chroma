using System;
using System.Collections.Generic;
using System.Drawing;
using System.Geometry;
using System.Linq;
using System.Numerics;
using Chroma;
using Chroma.Diagnostics;
using Chroma.Graphics;
using Chroma.Graphics.TextRendering;
using Chroma.Graphics.TextRendering.TrueType;
using Color = Chroma.Graphics.Color;

namespace GlyphTransforms
{
    public class GameCore : Game
    {
        private float _angle;

        private List<Color> _colors = new()
        {
            Color.Red,
            Color.Orange,
            Color.Yellow,
            Color.Lime,
            Color.DodgerBlue,
            Color.Indigo,
            Color.Purple
        };

        private Random _random = new();

        public GameCore() : base(new(false, false))
        {
        }

        protected override void Update(float delta)
        {
            _angle += 10 * delta;
        }

        private List<float> _floats = new();
        private List<Vector2> _pos = new();

        protected override void Draw(RenderContext context)
        {
            context.DrawString(
                "This text should be colored like a rainbow UwU",
                new Vector2(8),
                (d, _, i, p) =>
                {
                    d.Position = p;
                    d.Color = _colors[i % _colors.Count];
                }
            );

            context.DrawString(
                "This text should be wavy!",
                new Vector2(64),
                (d, _, i, p) =>
                {
                    var offsetY = 3 * MathF.Sin(_angle + (i * 4));
                    d.Position = p + new Vector2(0, offsetY);
                }
            );

            context.DrawString(
                "This text should be circley!",
                new Vector2(200),
                (d, _, i, _) =>
                {
                    // this example uses Bezier nuget library
                    // it's excellent for geometry calculations.
                    //
                    // also thanks to using .NET's System.Numerics
                    // types, interop between different libraries
                    // and Chroma is easy af. 
                    var circle = new Circle(160);
                    var pointOnCircle = circle.Position((i * 2 + _angle) / 100);
                    var offsetY = 3 * MathF.Sin(_angle + (i * 4));
                    pointOnCircle.Y += offsetY;

                    d.Position = pointOnCircle + new Vector2(300);
                    d.Rotation = _angle * 10;
                    d.Color = _colors[i % _colors.Count];
                }
            );

            context.DrawString(
                "This text should be rotated 90 degrees right.",
                new Vector2(120, 250),
                (d, c, i, p) =>
                {
                    var offsets = TrueTypeFont.Default.GetRenderOffsets(c);
                    var bounds = TrueTypeFont.Default.GetGlyphBounds(c);
                    TrueTypeFont.Default.GetGlyphControlBox(
                        c,
                        out _,
                        out _,
                        out var yMin,
                        out _
                    );

                    d.Position = new(p.Y - offsets.Y + bounds.Height + yMin, p.X);
                    d.Rotation = 90;
                }
            );

            var str = "spoopy text be like shakin";
            if (PerformanceCounter.LifetimeFrames % 10 == 0 || !_pos.Any())
            {
                _pos.Clear();
                _floats.Clear();
                
                for (var i = 0; i < str.Length; i++)
                {
                    _floats.Add(_random.Next(-20, 20));
                    _pos.Add(new Vector2(_random.Next(-2, 2), _random.Next(-2, 2)));
                }
            }

            context.DrawString(
                str,
                new Vector2(300, 340),
                (d, c, i, p) =>
                {
                    var bounds = TrueTypeFont.Default.GetGlyphBounds(c);

                    var origin = new Vector2(bounds.Width / 2f, bounds.Height / 2f);
                    
                    d.Position = p + _pos[i] + origin;
                    d.Rotation = _floats[i];
                    d.Origin = origin;
                }
            );

            var rect = new Rectangle(200, 200, 240, TrueTypeFont.Default.Height);
            context.Rectangle(ShapeMode.Fill, rect, Color.Red);

            var text = "this should be centered";
            var measure = TrueTypeFont.Default.Measure(text);
            var targetPos = new Vector2(
                rect.X + (rect.Width / 2) - measure.Width / 2,
                rect.Y + (rect.Height / 2) - measure.Height / 2
            );
            context.DrawString(text, targetPos);
        }
    }
}