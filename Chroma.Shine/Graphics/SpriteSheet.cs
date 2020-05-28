using System.Collections.Generic;
using System.Drawing;
using System.Numerics;

namespace Chroma.Graphics
{
    public class SpriteSheet : Sprite
    {
        private List<Rectangle> _sourceRectangles;
        private int _currentFrame;

        public int FrameWidth { get; }
        public int FrameHeight { get; }

        public int TotalFrames => _sourceRectangles.Count;

        public int CurrentFrame
        {
            get => _currentFrame;
            set
            {
                if (WrapFrameIndexOnOverflow)
                    _currentFrame = value % _sourceRectangles.Count;
                else
                    _currentFrame = value;
            }
        }

        public bool WrapFrameIndexOnOverflow { get; set; } = true;

        public SpriteSheet(string filePath, int frameWidth, int frameHeight) : base(filePath)
        {
            FrameWidth = frameWidth;
            FrameHeight = frameHeight;

            CalculateFrameRectangles();
        }

        public SpriteSheet(Texture texture, int frameWidth, int frameHeight) : base(texture)
        {
            FrameWidth = frameWidth;
            FrameHeight = frameHeight;

            CalculateFrameRectangles();
        }

        public override void Draw(RenderContext context)
        {
            if (Shearing != Vector2.Zero)
            {
                context.Transform.Push();
                context.Transform.Shear(Shearing);
            }

            context.DrawTexture(Texture, Position, Scale, Origin, Rotation, _sourceRectangles[CurrentFrame]);

            if (Shearing != Vector2.Zero)
            {
                context.Transform.Pop();
            }
        }

        private void CalculateFrameRectangles()
        {
            _sourceRectangles = new List<Rectangle>();

            var totalFramesH = Texture.Width / FrameWidth;
            var totalFramesV = Texture.Height / FrameHeight;

            for (var y = 0; y < totalFramesV; y++)
            {
                for (var x = 0; x < totalFramesH; x++)
                {
                    _sourceRectangles.Add(
                        new Rectangle(
                            x * FrameWidth,
                            y * FrameHeight,
                            FrameWidth,
                            FrameHeight
                        )
                    );
                }
            }
        }
    }
}