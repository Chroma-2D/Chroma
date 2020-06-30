using System;
using Chroma;
using Chroma.Diagnostics.Logging;

namespace ChromaGame
{
    internal class GameCore : Game
    {
        private Log Log { get; } = LogManager.GetForCurrentAssembly();

        internal GameCore()
        {
            Log.Info("Hello, world!");
        }
    }
}
