using Chroma.Graphics;

namespace PrimitiveShapes
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