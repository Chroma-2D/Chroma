using Chroma.Graphics;
using System;
using System.Collections.Generic;
using System.Text;

namespace Chroma.ExampleApp
{
    public class ExampleGame : Game
    {
        private Vector2 _rectPosition;
        private int _hSign = 1;
        private int _vSign = 1;

        public ExampleGame()
        {
            _rectPosition = new Vector2(10, 10);
        }

        protected override void Draw(RenderContext context)
        {
            context.Clear(Color.CornflowerBlue);

            context.LineThickness = 1;
            context.Rectangle(RectangleMode.Fill, _rectPosition, new Size(10, 10), Color.HotPink);
        }

        protected override void Update(float delta)
        {
            Console.WriteLine(delta);

            if (_rectPosition.X + 10 >= Window.Size.Width || _rectPosition.X - 1 < 0)
                _hSign *= -1;

            if (_rectPosition.Y + 10 >= Window.Size.Height || _rectPosition.Y - 1 < 0)
                _vSign *= -1;

            var dy = (100 * _vSign * delta);
            var dx = (100 * _hSign * delta);

            _rectPosition = new Vector2(_rectPosition.X + dx, _rectPosition.Y + dy);
        }
    }
}
