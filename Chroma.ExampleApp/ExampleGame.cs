using Chroma.Graphics;
using Chroma.Input.EventArgs;
using System;
using System.Collections.Generic;
using System.Numerics;

namespace Chroma.ExampleApp
{
    public class ExampleGame : Game
    {
        private Color[] _palette = new[]
        {
            Color.Red,
            Color.Cyan,
            Color.Gray,
            Color.DarkRed,
            Color.IndianRed,
            Color.MediumVioletRed,
            Color.OrangeRed,
            Color.PaleVioletRed,
        };
        
        private List<float> _heights = new List<float>();
        private float _shift = 0;

        public ExampleGame()
        {
            Graphics.VSyncEnabled = true;
            Window.GoWindowed(1024, 640);
        }

        protected override void Update(float delta)
        {
            Window.Properties.Title = $"FPS: {Window.FPS}";

            _heights.Clear();
            for (var i = 0; i < 64; i++)
            {
                _heights.Add(MathF.Sin((_shift += 0.1f) % 360 * delta));
            }
        }

        protected override void Draw(RenderContext context)
        {
            for (var i = 0; i < _heights.Count; i++)
            {
                context.Rectangle(
                    ShapeMode.Fill,
                    new Vector2(8 * i, 320),
                    7, 80 * _heights[i], _palette[i % _palette.Length]
                );
            }
        }

        protected override void KeyPressed(KeyEventArgs e)
        {
            
        }
    }
}