using System.Reflection;
using Chroma.Graphics;
using Chroma.Graphics.TextRendering;

namespace Chroma
{
    internal class EmbeddedAssets
    {
        internal static TrueTypeFont DefaultFont { get; private set; }
        internal static Texture LogoTexture { get; private set; }
        internal static Texture DefaultIconTexture { get; private set; }
        internal static Texture BetaEmblemTexture { get; private set; }

        static EmbeddedAssets()
        {
            using var fontResourceStream = Assembly.GetExecutingAssembly()
                .GetManifestResourceStream("Chroma.Resources.default.ttf");

            using var logoResourceStream = Assembly.GetExecutingAssembly()
                .GetManifestResourceStream("Chroma.Resources.logo.png");

            using var defaultIconStream = Assembly.GetExecutingAssembly()
                .GetManifestResourceStream("Chroma.Resources.deficon.png");

            using var betaEmblemStream = Assembly.GetExecutingAssembly()
                .GetManifestResourceStream("Chroma.Resources.beta.png");

            DefaultFont = new TrueTypeFont(fontResourceStream, 16);
            LogoTexture = new Texture(logoResourceStream);
            DefaultIconTexture = new Texture(defaultIconStream);
            DefaultIconTexture.FilteringMode = TextureFilteringMode.NearestNeighbor;

            BetaEmblemTexture = new Texture(betaEmblemStream);
        }
    }
}