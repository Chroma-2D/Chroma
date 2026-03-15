namespace Chroma;

using Chroma.Diagnostics.Logging;

public class GameStartupOptions(
    bool constructDefaultScene = true,
    bool useBootSplash = true,
    int msaaSamples = 0,
    LogLevel frameworkLogLevel =
#if !DEBUG
        LogLevel.Info | LogLevel.Warning | LogLevel.Error
#else
        LogLevel.Everything
#endif
)
{
    public bool ConstructDefaultScene { get; } = constructDefaultScene;
    public bool UseBootSplash { get; } = useBootSplash;

    public int MsaaSamples { get; } = msaaSamples;
    public LogLevel FrameworkLogLevel { get; } = frameworkLogLevel;

    public static readonly GameStartupOptions Default = new();
}