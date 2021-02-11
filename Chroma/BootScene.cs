using System;
using System.Collections.Generic;
using System.Drawing;
using System.Numerics;
using Chroma.Graphics;

namespace Chroma
{
    internal class BootScene
    {
        private const int AnimationFrameWidth = 64;
        private const int AnimationFrameHeight = 64;
        private const int TotalAnimationFrames = 300;
        
        private readonly Game _game;
        
        private int _currentFrame;
        private List<Rectangle> _frames;

        internal event EventHandler Finished;

        internal BootScene(Game game)
        {
            _game = game;
            
            EmbeddedAssets.BootSheetTexture.FilteringMode = TextureFilteringMode.NearestNeighbor;
            CalculateAnimationFrames();
        }

        internal void FixedUpdate(float delta)
        {
            if (_currentFrame >= _frames.Count)
                return;
            
            _currentFrame++;

            if (_currentFrame >= _frames.Count)
            {
                Finished?.Invoke(this, EventArgs.Empty);
                _frames.Clear();
            }
        }

        internal void Draw(RenderContext context)
        {
            context.DrawTexture(
                EmbeddedAssets.BootSheetTexture,
                _game.Window.Center - new Vector2(32 * 6),
                new(6), 
                Vector2.Zero, 
                0, 
                _frames[_currentFrame]
           );
        }

        private void CalculateAnimationFrames()
        {
            _frames = new List<Rectangle>();

            for (var y = 0; y < EmbeddedAssets.BootSheetTexture.Height; y += AnimationFrameHeight)
            {
                for (var x = 0; x < EmbeddedAssets.BootSheetTexture.Width; x += AnimationFrameWidth)
                {
                    _frames.Add(new Rectangle(x, y, AnimationFrameWidth, AnimationFrameHeight));

                    if (_frames.Count >= TotalAnimationFrames)
                        break;
                }
            }
        }
    }
}