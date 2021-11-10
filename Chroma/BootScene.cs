using System;
using System.Numerics;
using Chroma.Graphics;

namespace Chroma
{
    internal class BootScene
    {
        private const int FrameCount = 301;
        
        private readonly Game _game;
        private readonly Vector2 _bootTextTextureOffset = new(0, 270);

        private int _currentFrame;

        private EmbeddedAssets.BootAnimationInfo CurrentBootTextProperties
            => EmbeddedAssets.BootAnimationData[0][_currentFrame];

        private EmbeddedAssets.BootAnimationInfo CurrentBootWheelProperties
            => EmbeddedAssets.BootAnimationData[1][_currentFrame];

        private EmbeddedAssets.BootAnimationInfo CurrentBootLogoProperties
            => EmbeddedAssets.BootAnimationData[2][_currentFrame];

        internal event EventHandler Finished;

        internal BootScene(Game game)
        {
            _game = game;
        }

        internal void FixedUpdate(float delta)
        {
            EmbeddedAssets.BootWheelTexture.ColorMask = new Color(1f, 1f, 1f, CurrentBootWheelProperties.Opacity);
            EmbeddedAssets.BootLogoTexture.ColorMask = new Color(1f, 1f, 1f, CurrentBootLogoProperties.Opacity);
            EmbeddedAssets.BootTextTexture.ColorMask = new Color(1f, 1f, 1f, CurrentBootTextProperties.Opacity);

            if (_currentFrame >= FrameCount)
                return;

            _currentFrame++;

            if (_currentFrame >= FrameCount)
                Finished?.Invoke(this, EventArgs.Empty);
        }

        internal void Draw(RenderContext context)
        {
            context.DrawTexture(
                EmbeddedAssets.BootWheelTexture,
                _game.Window.Center,
                Vector2.One,
                EmbeddedAssets.BootWheelTexture.Center,
                CurrentBootWheelProperties.Rotation
            );

            context.DrawTexture(
                EmbeddedAssets.BootLogoTexture,
                _game.Window.Center,
                new Vector2(CurrentBootLogoProperties.Scale),
                EmbeddedAssets.BootLogoTexture.Center,
                0
            );

            context.DrawTexture(
                EmbeddedAssets.BootTextTexture,
                _game.Window.Center + _bootTextTextureOffset,
                Vector2.One,
                EmbeddedAssets.BootTextTexture.Center,
                0
            );
        }
    }
}