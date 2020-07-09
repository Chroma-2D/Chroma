using Chroma.Graphics;

namespace EmptyProject
{
    internal class Program
    {
        internal static void Main(string[] args)
        {
            GraphicsManager.MultiSamplingPrecision = 4;
            new GameCore().Run();
        }
    }
}