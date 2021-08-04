using System;
using Chroma;
using Chroma.Diagnostics.Logging;

namespace ChromaGame
{
    internal class GameCore : Game
    {
        private Log Log { get; } = LogManager.GetForCurrentAssembly();
                            // uncomment this to disable construction of
                            // the default scene and splash screen
        internal GameCore() // : base(new(false, false))
        {
            Log.Info("Hello, world!");
        }
    }
}
