﻿using System.Numerics;
using Chroma;
using Chroma.Graphics;
using Chroma.Input;
using Chroma.Input.EventArgs;

namespace RenderTargets
{
    public class GameCore : Game
    {
        private RenderTarget _tgt;

        private float _rotation;
        private Vector2 _position = new Vector2(32);
        
        protected override void LoadContent()
        {
            _tgt = new RenderTarget(
                Window.Properties.Width / 2,
                Window.Properties.Height / 2
            );
        }

        protected override void Update(float delta)
        {
            if (Keyboard.IsKeyDown(KeyCode.W))
            {
                _position.Y -= 50 * delta;
            }
            else if (Keyboard.IsKeyDown(KeyCode.S))
            {
                _position.Y += 50 * delta;
            }
            
            if (Keyboard.IsKeyDown(KeyCode.A))
            {
                _position.X -= 50 * delta;
            }
            else if (Keyboard.IsKeyDown(KeyCode.D))
            {
                _position.X += 50 * delta;
            }    
        }
        
        protected override void Draw(RenderContext context)
        {
            context.RenderTo(_tgt, () =>
            {
                context.Clear(Color.Black);
                context.Rectangle(ShapeMode.Stroke, _position, 64, 64, Color.Cyan);
                context.DrawString("This was rendered inside a render target.", new Vector2(16));
            });

            context.Clear(Color.Crimson);

            context.DrawTexture(
                _tgt, 
                _tgt.Center + new Vector2(Window.Properties.Width / 2 - _tgt.Width / 2, Window.Properties.Height / 2 - _tgt.Width /2),
                Vector2.One, 
                _tgt.Center, 
                _rotation
            );
            
            context.DrawString(
                "Use left and right cursor keys to rotate the render target around its center.\n" +
                "Use <W> <A> <S> <D> to move the cyan rectangle around the render target.",
                new Vector2(8)
            );
        }

        protected override void KeyPressed(KeyEventArgs e)
        {
            if (e.KeyCode == KeyCode.Left)
                _rotation -= 10;
            else if (e.KeyCode == KeyCode.Right)
                _rotation += 10;
        }
    }
}