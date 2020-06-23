using System.Collections.Generic;
using System.Drawing;
using System.Numerics;
using Chroma;
using Chroma.Graphics;
using Color = Chroma.Graphics.Color;

namespace PrimitiveShapes
{
    public class GameCore : Game
    {
        protected override void Draw(RenderContext context)
        {
            context.Arc(
                ShapeMode.Fill,
                new Vector2(32),
                radius: 32,
                startAngle: 0,
                endAngle: 90,
                Color.HotPink
            );

            context.LineThickness = 2;
            context.Circle(
                ShapeMode.Stroke,
                new Vector2(64, 32),
                radius: 32,
                Color.Lime
            );
            context.LineThickness = 1;

            context.Ellipse(
                ShapeMode.Fill,
                new Vector2(96, 96),
                new Vector2(16, 48),
                rotation: 45f,
                Color.Aqua
            );

            context.LineThickness = 4;
            context.Line(
                new Vector2(120, 120),
                new Vector2(48, 48),
                Color.Yellow
            );
            context.LineThickness = 1;
            context.DrawString("<- A whole bunch\nof primitives", new Vector2(160, 64));

            context.DrawString("A polygon:", new Vector2(170, 170));
            context.Polygon(
                ShapeMode.Fill,
                new List<Point>
                {
                    new Point(200, 200),
                    new Point(232, 200),
                    new Point(260, 230),
                    new Point(243, 240),
                    new Point(160, 220)
                }, Color.Purple
            );

            context.DrawString("A polyline:", new Vector2(270, 270));
            context.Polyline(
                new List<Point>
                {
                    new Point(300, 300),
                    new Point(310, 310),
                    new Point(320, 295),
                    new Point(330, 315),
                    new Point(340, 290),
                    new Point(350, 320)
                },
                Color.DodgerBlue,
                closeLoop: false
            );

            context.Rectangle(
                ShapeMode.Fill,
                new Vector2(400, 400),
                100, 
                100, 
                Color.Red
            );
            context.DrawString("Wonderful\nrectangle.", new Vector2(410, 410));
        }
    }
}