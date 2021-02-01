namespace Chroma
{
    public class GameStartupOptions
    {
        public bool ConstructDefaultScene { get; }
        public int MsaaSamples { get; }

        public GameStartupOptions(bool constructDefaultScene = true, int msaaSamples = 0)
        {
            ConstructDefaultScene = constructDefaultScene;
            MsaaSamples = msaaSamples;
        }
    }
}