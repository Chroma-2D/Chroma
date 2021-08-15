using System;
using System.Collections.Generic;
using System.Geometry;
using System.Numerics;
using Chroma;
using Chroma.Graphics;
using Chroma.Graphics.TextRendering;
using Chroma.Graphics.TextRendering.TrueType;

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
        
        public GameCore() : base(new(false, false))
        {
        }

        protected override void Update(float delta)
        {
            _angle += 10 * delta;
        }

        protected override void Draw(RenderContext context)
        {
            context.DrawString(
                "This text should be colored like a rainbow UwU",
                new Vector2(8),
                (_, i, p) =>
                {
                    return new GlyphTransformData
                    {
                        Position = p,
                        Color = _colors[i % _colors.Count]
                    };
                }
            );

            context.DrawString(
                "This text should be wavy!",
                new Vector2(64),
                (_, i, p) =>
                {
                    var offsetY = 3 * MathF.Sin(_angle + (i * 4));
                    return new GlyphTransformData
                    {
                        Position = p + new Vector2(0, offsetY)
                    };
                }
            );

            context.DrawString(
                "This text should be circley!",
                new Vector2(200),
                (_, i, _) =>
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

                    return new GlyphTransformData
                    {
                        Position = pointOnCircle + new Vector2(300),
                        Rotation = _angle * 10,
                        Color = _colors[i % _colors.Count]
                    };
                }
            );
            
            context.DrawString(
                "This text should be rotated 90 degrees right.",
                new Vector2(120, 250),
                (c, i, p) =>
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
                    
                    return new GlyphTransformData
                    {
                        Position = new(p.Y - offsets.Y + bounds.Height + yMin, p.X),
                        Rotation = 90
                    };
                }
            );
        }
    }
}