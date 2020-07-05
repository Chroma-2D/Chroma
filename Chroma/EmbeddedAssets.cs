using System;
using System.Linq;
using System.Reflection;
using Chroma.Graphics;
using Chroma.Graphics.TextRendering;

namespace Chroma
{
    internal class EmbeddedAssets
    {
        internal const string LogoResourceKey = "Chroma.Resources.logo.png";
        internal const string BetaEmblemResourceKey = "Chroma.Resources.beta.png";
        internal const string DefaultIconResourceKey = "Chroma.Resources.deficon.png";
        internal const string DefaultFontResourceKey = "Chroma.Resources.default.ttf";
        
        private static Texture _logo;
        private static Texture _betaEmblem;
        private static Texture _defaultIcon;
        private static TrueTypeFont _defaultFont;

        internal static Texture LogoTexture => LazyLoad(ref _logo, LogoResourceKey);
        internal static Texture BetaEmblemTexture => LazyLoad(ref _betaEmblem, BetaEmblemResourceKey);
        internal static Texture DefaultIconTexture => LazyLoad(ref _defaultIcon, DefaultIconResourceKey);
        internal static TrueTypeFont DefaultFont => LazyLoad(ref _defaultFont, DefaultFontResourceKey, 16, null);

        private static T LazyLoad<T>(ref T field, string resourceKey, params object[] args)
        {
            if (field == null)
            {
                using var resourceStream = Assembly.GetExecutingAssembly()
                    .GetManifestResourceStream(resourceKey);

                var actualArgs = new[] {resourceStream}.Concat(args).ToArray();
                field = (T)Activator.CreateInstance(typeof(T), actualArgs);
            }

            return field;
        }
    }
}