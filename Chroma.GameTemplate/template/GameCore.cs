namespace ChromaGame
    
using System;
using Chroma;
using Chroma.Diagnostics.Logging;

internal class GameCore() : Game(/*(new(false, false)*/) // Uncomment to disable splash screen and default asset init.
{
    private Log Log { get; } = LogManager.GetForCurrentAssembly();

    protected override void Initialize(IContentProvider content)
    {
        Log.Info("Hello, world!!");
    }
}