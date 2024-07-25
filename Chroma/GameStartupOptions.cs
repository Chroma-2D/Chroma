namespace Chroma;

public class GameStartupOptions
{
    public bool ConstructDefaultScene { get; }
    public bool UseBootSplash { get; }
        
    public int MsaaSamples { get; }

    public GameStartupOptions(bool constructDefaultScene = true, bool useBootSplash = true, int msaaSamples = 0)
    {
        ConstructDefaultScene = constructDefaultScene;
        MsaaSamples = msaaSamples;
        UseBootSplash = useBootSplash;
    }
}