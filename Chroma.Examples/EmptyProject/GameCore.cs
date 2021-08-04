using Chroma;
using Chroma.Diagnostics.Logging;
using Chroma.Graphics;

namespace EmptyProject
{
    internal class GameCore : Game
    {
        private Log Log { get; } = LogManager.GetForCurrentAssembly();

        internal GameCore()
        {
            Log.Info("Hello, world!");
        }

        protected override void Draw(RenderContext context)
        {
        }
    }
}