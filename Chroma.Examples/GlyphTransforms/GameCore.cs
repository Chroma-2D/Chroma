﻿using System;
using System.Collections.Generic;
using System.Geometry;
using System.Numerics;
using Chroma;
using Chroma.Graphics;
using Chroma.Graphics.TextRendering;

namespace GlyphTransforms
{
    public class GameCore : Game
    {
        private float _angle;

        private List<Color> _colors = new List<Color>
        {
            Color.Red,
            Color.Orange,
            Color.Yellow,
            Color.Lime,
            Color.DodgerBlue,
            Color.Indigo,
            Color.Purple
        };

        protected override void Update(float delta)
        {
            _angle += 10 * delta;
            _angle %= 360;
        }

        protected override void Draw(RenderContext context)
        {
            context.DrawString(
                "This text should be colored like a rainbow UwU",
                new Vector2(8),
                (c, i, p, g) =>
                {
                    var transform = new GlyphTransformData(p);

                    transform.Color = _colors[i % _colors.Count];
                    return transform;
                }
            );

            context.DrawString(
                "This text should be wavy!",
                new Vector2(64),
                (c, i, p, g) =>
                {
                    var offsetY = 3 * MathF.Sin(_angle + (i * 4));

                    var pos = new Vector2(p.X, p.Y);
                    pos.Y += offsetY;

                    return new GlyphTransformData(pos);
                }
            );

            context.DrawString(
                "This text should be circley!",
                new Vector2(200),
                (c, i, p, g) =>
                {
                    // this example uses Bezier nuget library
                    // it's excellent for geometry calculations.
                    var circle = new Circle(160);
                    var pointOnCircle = circle.Position((i * 2 + _angle) / 100);
                    var offsetY = 3 * MathF.Sin(_angle + (i * 4));
                    pointOnCircle.Y += offsetY;

                    return new GlyphTransformData(pointOnCircle + new Vector2(300))
                    {
                        Rotation = _angle * 10,
                        Color = _colors[i % _colors.Count]
                    };
                }
            );
        }
    }
}