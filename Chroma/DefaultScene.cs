using System;
using System.Numerics;
using System.Reflection;
using Chroma.Graphics;
using Chroma.Graphics.TextRendering;

namespace Chroma
{
    internal class DefaultScene
    {
        private Game Game { get; }
        
        private static readonly string _welcomeMessage =
            "Welcome to Chroma Framework.\nTo get started, override Draw and Update methods.";
        private static readonly string _versionString = $"v{Assembly.GetExecutingAssembly().GetName().Version}";
        private float _betaEmblemHue;

        internal DefaultScene(Game game)
            => Game = game;

        internal void Draw(RenderContext context)
        {
            if (EmbeddedAssets.LogoTexture == null || EmbeddedAssets.LogoTexture.Disposed)
                return;

            context.Clear(Color.Black);
            context.DrawTexture(
                EmbeddedAssets.LogoTexture,
                Game.Window.Center - EmbeddedAssets.LogoTexture.Center,
                Vector2.One,
                Vector2.Zero,
                0f
            );

            context.DrawTexture(
                EmbeddedAssets.BetaEmblemTexture,
                Game.Window.Center - EmbeddedAssets.LogoTexture.Center + new Vector2(8),
                Vector2.One,
                Vector2.Zero,
                0f
            );

            context.DrawString(
                _welcomeMessage,
                new Vector2(8), (_, i, p) =>
                {
                    var ranges = _welcomeMessage.FindWordRanges("Draw", "Update");
                    var color = Color.White;
                    var yOff = 0f;
                    
                    foreach (var range in ranges)
                    {
                        if (range.Includes(i))
                        {
                            color = Color.DodgerBlue;
                            yOff = 2 * MathF.Sin(0.25f * (_betaEmblemHue + (i * 1.25f)));
                        }
                    }

                    return new GlyphTransformData
                    {
                        Position = p + new Vector2(0, yOff),
                        Color = color
                    };
                }
            );

            var measure = EmbeddedAssets.DefaultFont.Measure(_versionString);
            context.DrawString(
                _versionString,
                new Vector2(
                    Game.Window.Size.Width - measure.Width - 8,
                    Game.Window.Size.Height - measure.Height - 8
                )
            );
        }

        internal void Update(float delta)
        {
            _betaEmblemHue += 40 * delta;
            EmbeddedAssets.BetaEmblemTexture.ColorMask = Color.FromHSV(_betaEmblemHue, 1f, 0.85f);
        }
    }
}