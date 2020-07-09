using Chroma.Graphics;

namespace CustomContentProvider
{
    internal class Program
    {
        internal static void Main(string[] args)
        {
            GraphicsManager.MultiSamplingEnabled = true;
            GraphicsManager.MultiSamplingPrecision = 16;
            
            new GameCore().Run();
        }
    }
}