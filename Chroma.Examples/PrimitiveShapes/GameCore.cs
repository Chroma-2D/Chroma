using System.Collections.Generic;
using System.Drawing;
using System.Numerics;
using Chroma;
using Chroma.Graphics;
using Chroma.Input;
using Color = Chroma.Graphics.Color;

namespace PrimitiveShapes
{
    public class GameCore : Game
    {
        private Vector2 _cursorPosition;

        public GameCore() : base(new(false, true, 8))
        {
            Cursor.IsVisible = false;
            Graphics.VerticalSyncMode = VerticalSyncMode.None;
        }

        protected override void Draw(RenderContext context)
        {
            context.Rectangle(ShapeMode.Stroke, 0, 0, 32, 32, Color.Cyan);
            context.Line(0, 120, 0, 180, Color.Red);
            context.Line(Window.Width - 1, 120, Window.Width - 1, 180, Color.Red);
            context.Line(48, 0, 96, 0, Color.Lime);
            context.Line(48, Window.Height - 1, 96, Window.Height - 1, Color.Gold);
            
            context.Arc(
                ShapeMode.Fill,
                new Vector2(256, 32),
                radius: 32,
                startAngle: 0,
                endAngle: 90,
                Color.HotPink
            );

            RenderSettings.LineThickness = 2;
            context.Circle(
                ShapeMode.Stroke,
                new Vector2(296, 32),
                radius: 32,
                Color.Lime
            );
            RenderSettings.LineThickness = 1;

            context.Ellipse(
                ShapeMode.Fill,
                new Vector2(296, 96),
                new Vector2(16, 48),
                rotation: 45f,
                Color.Aqua
            );

            RenderSettings.LineThickness = 4;
            context.Line(
                new Vector2(120, 120),
                new Vector2(48, 48),
                Color.Yellow
            );
            RenderSettings.LineThickness = 1;
            context.Polygon(
                ShapeMode.Fill,
                new()
                {
                    new(200, 200),
                    new(232, 200),
                    new(260, 230),
                    new(243, 240),
                    new(160, 220)
                }, Color.Purple
            );

            context.Polyline(
                new()
                {
                    new(300, 300),
                    new(310, 310),
                    new(320, 295),
                    new(330, 315),
                    new(340, 290),
                    new(350, 320)
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

            RenderSettings.LineThickness = 1;
            context.Rectangle(
                ShapeMode.Stroke,
                _cursorPosition - new Vector2(1),
                34, 34,
                Color.White
            );

            context.Rectangle(
                ShapeMode.Fill,
                _cursorPosition,
                32, 32, Color.Green
            );

            context.DrawString(
                $"Shape blending [F1]: {RenderSettings.ShapeBlendingEnabled}\n" +
                $"Multisampling [F2] {RenderSettings.MultiSamplingEnabled}\n" +
                "Hit [F3] to set blending functions.",
                new Vector2(16, 16)
            );
        }

        protected override void KeyPressed(KeyEventArgs e)
        {
            switch (e.KeyCode)
            {
                case KeyCode.F1:
                    RenderSettings.ShapeBlendingEnabled = !RenderSettings.ShapeBlendingEnabled;
                    break;

                case KeyCode.F2:
                    RenderSettings.MultiSamplingEnabled = !RenderSettings.MultiSamplingEnabled;
                    break;

                case KeyCode.F3:
                    RenderSettings.SetShapeBlendingFunctions(
                        destinationColorBlend: BlendingFunction.SourceAlpha,
                        destinationAlphaBlend: BlendingFunction.DestinationColor,
                        sourceColorBlend: BlendingFunction.OneMinusDestinationAlpha,
                        sourceAlphaBlend: BlendingFunction.OneMinusSourceAlpha
                    );
                    break;
            }
        }

        protected override void MouseMoved(MouseMoveEventArgs e)
        {
            _cursorPosition = e.Position;
        }
    }
}