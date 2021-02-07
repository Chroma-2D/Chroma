using System.Drawing;
using System.Numerics;
using Chroma.Graphics;
using Chroma.Input;
using Color = Chroma.Graphics.Color;

namespace Pong
{
    public class Board
    {
        public Size Size { get; }

        public int LeftScore;
        public int RightScore;

        private Paddle _leftPaddle;
        private Paddle _rightPaddle;
        private Ball _ball;

        public Board(Size size)
        {
            Size = size;

            Assets.Stretchy.VirtualResolution = new Size(
                8,
                size.Height
            );

            Assets.Stretchy.FilteringMode = TextureFilteringMode.NearestNeighbor;

            _leftPaddle = new Paddle(this)
            {
                UpKey = KeyCode.W,
                DownKey = KeyCode.S,
            };
            _leftPaddle.Position = new(
                48,
                Size.Height / 2f - _leftPaddle.Size.Y / 2
            );

            _rightPaddle = new Paddle(this);
            _rightPaddle.Position = new(
                Size.Width - 48 - _rightPaddle.Size.X,
                Size.Height / 2f - _rightPaddle.Size.Y / 2
            );

            _ball = new Ball(this);
        }

        public void Draw(RenderContext context)
        {
            context.DrawTexture(
                Assets.Stretchy,
                new Vector2(
                    Size.Width / 2f - Assets.Stretchy.VirtualResolution!.Value.Width / 2f,
                    -12
                ),
                Vector2.One,
                Vector2.Zero,
                0
            );

            var scoreStr = $"{LeftScore}  {RightScore}";
            var scoreSize = Assets.ScoreFont.Measure(scoreStr);

            context.DrawString(
                Assets.ScoreFont,
                scoreStr,
                new Vector2(
                    Size.Width / 2f,
                    Size.Height / 2f
                ) - new Vector2(scoreSize.Width / 2f, scoreSize.Height / 2f),
                Color.White
            );

            _leftPaddle.Draw(context);
            _rightPaddle.Draw(context);
            _ball.Draw(context);
        }

        public void Update(float delta)
        {
            if (Keyboard.IsKeyDown(KeyCode.Space))
            {
                _ball.CanMove = true;
            }

            _leftPaddle.Update(delta);
            _rightPaddle.Update(delta);
            _ball.Update(delta);

            if (_ball.CollidesWithPaddle(_leftPaddle))
            {
                _ball.BounceFromPaddle(_leftPaddle);
            }
            else if (_ball.CollidesWithPaddle(_rightPaddle))
            {
                _ball.BounceFromPaddle(_rightPaddle);
            }
            else if (_ball.CollidesWithBoardSide())
            {
                _ball.BounceFromSide();
            }

            if (_ball.CollidesWithEndOfPlayfield(out var left))
            {
                if (left)
                {
                    RightScore++;
                }
                else
                {
                    LeftScore++;
                }

                _ball.Center();
                _ball.CanMove = false;
            }
        }
    }
}