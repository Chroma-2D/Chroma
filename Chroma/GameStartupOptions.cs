namespace Chroma;

public class GameStartupOptions
{
    public bool ConstructDefaultScene { get; }
    public bool UseBootSplash { get; }
        
    public int MsaaSamples { get; }

    public static readonly GameStartupOptions Default = new(); 

    public GameStartupOptions(bool constructDefaultScene = true, bool useBootSplash = true, int msaaSamples = 0)
    {
        ConstructDefaultScene = constructDefaultScene;
        MsaaSamples = msaaSamples;
        UseBootSplash = useBootSplash;
    }
}