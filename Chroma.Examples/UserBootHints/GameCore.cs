namespace UserBootHints;

using Chroma;
using Chroma.Diagnostics.Logging;

internal class GameCore : Game
{
    private Log Log { get; } = LogManager.GetForCurrentAssembly();

    internal GameCore()
    {
        Log.Info("Hello, world!");
    }
}