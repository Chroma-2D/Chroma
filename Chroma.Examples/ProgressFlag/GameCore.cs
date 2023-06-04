using System.Collections.Generic;
using System.Numerics;
using Chroma;
using Chroma.Graphics;

namespace ProgressFlag
{
    public class GameCore : Game
    {
        private readonly List<Color> _flagColors = new()
        {
            new(226, 32, 22),
            new(242, 137, 23),
            new(239, 229, 36),
            new(120, 184, 42),
            new(44, 88, 164),
            new(109, 35, 128)
        };

        private readonly List<Color> _chevronColors = new()
        {
            new(29, 29, 27),
            new(148, 85, 22),
            new(123, 204, 229),
            new(244, 174, 200),
            new(255, 255, 255),
            new(253, 216, 23)
        };

        private readonly Color _circleColor = new(102, 51, 139);

        private int _stripeHeight;
        private int _chevronWidth = 90;
        private Vector2 _centerPoint;

        public GameCore()
            : base(new(false, false, 4))
        {
            RenderSettings.MultiSamplingEnabled = true;
            Window.Mode.SetWindowed(1200, 768);
        }

        protected override void Update(float delta)
        {
            _stripeHeight = Window.Height / _flagColors.Count;
            _centerPoint = Window.Center + new Vector2(Window.Width / 6.66f, 0);
        }

        protected override void Draw(RenderContext context)
        {
            DrawStripes(context);
            DrawChevrons(context);
            DrawCircle(context);
        }

        private void DrawStripes(RenderContext context)
        {
            for (var i = 0; i < _flagColors.Count; i++)
            {
                context.Rectangle(
                    ShapeMode.Fill,
                    0,
                    i * _stripeHeight,
                    Window.Width,
                    _stripeHeight,
                    _flagColors[i]
                );
            }
        }

        private void DrawChevrons(RenderContext context)
        {
            for (var i = 0; i < _chevronColors.Count; i++)
            {
                DrawChevron(
                    context, 
                    _chevronColors[i],
                    -i * _chevronWidth
                );
            }
        }

        //
        // "can't draw it with triangles" ~ncommander
        // 
        private void DrawChevron(RenderContext context, Color color, int xOffset = 0)
        {
            context.Triangle(
                ShapeMode.Fill,
                new Vector2(xOffset, -(Window.Height / 2.10f)),
                _centerPoint + new Vector2(xOffset, 0),
                new(xOffset, Window.Height + Window.Height / 2.10f),
                color
            );
        }

        private void DrawCircle(RenderContext context)
        {
            RenderSettings.LineThickness = 12 * ((float)Window.Width / Window.Height);
            
            context.Ellipse(
                ShapeMode.Stroke,
                new Vector2(Window.Width / 9.5f, Window.Center.Y),
                new Vector2(Window.Width / 12.63f, Window.Height / 8.08f),
                0,
                _circleColor
            );
        }
    }
}