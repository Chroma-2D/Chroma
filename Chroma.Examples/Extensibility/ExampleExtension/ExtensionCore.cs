using System.Numerics;
using Chroma;
using Chroma.Diagnostics;
using Chroma.Diagnostics.Logging;
using Chroma.Extensibility;
using Chroma.Graphics;

namespace ExampleExtension
{
    [EntryPoint]
    internal sealed class ExtensionCore
    {
        private Game _game;

        private bool _beforeUpdateRan;
        private bool _afterUpdateRan;
        private bool _beforeFixedUpdateRan;
        private bool _afterFixedUpdateRan;

        private Log _log = LogManager.GetForCurrentAssembly();

        internal ExtensionCore(Game game)
        {
            _game = game;
        }

        [BeforeDraw]
        internal bool BeforeDrawHook(RenderContext context)
        {
            context.DrawString(
                $"This text was drawn with a BeforeDraw hook! ({600 - PerformanceCounter.LifetimeFrames})",
                new Vector2(16, 16),
                Color.LimeGreen
            );

            return PerformanceCounter.LifetimeFrames > 600;
        }

        [AfterDraw]
        internal void AfterDrawHook(RenderContext context)
        {
            context.DrawString(
                "This text was drawn with an AfterDraw hook!",
                new Vector2(16, 40),
                Color.HotPink
            );
        }

        [BeforeUpdate]
        internal bool BeforeUpdateHook(float delta)
        {
            if (!_beforeUpdateRan)
            {
                _log.Info("Hi, I'm a BeforeUpdate hook!");
                _beforeUpdateRan = true;
            }

            return true;
        }

        [AfterUpdate]
        internal void AfterUpdateHook(float delta)
        {
            if (!_afterUpdateRan)
            {
                _log.Info("Hi, I'm an AfterUpdate hook!");
                _afterUpdateRan = true;
            }
        }

        [BeforeFixedUpdate]
        internal bool BeforeFixedUpdateHook(float delta)
        {
            if (!_beforeFixedUpdateRan)
            {
                _log.Info("Hi, I'm a BeforeFixedUpdate hook!");
                _beforeFixedUpdateRan = true;
            }

            return true;
        }

        [AfterFixedUpdate]
        internal void AfterFixedUpdateHook(float delta)
        {
            if (!_afterFixedUpdateRan)
            {
                _log.Info("Hi, I'm an AfterFixedUpdate hook!");
                _afterFixedUpdateRan = true;
            }
        }
    }
}