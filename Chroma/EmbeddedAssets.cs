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
        internal const string DummyFixResourceKey = "Chroma.Resources.dummy.png";
        internal const string DefaultIconResourceKey = "Chroma.Resources.deficon.png";
        internal const string DefaultFontResourceKey = "Chroma.Resources.default.ttf";
        
        private static Texture _logo;
        private static Texture _betaEmblem;
        private static Texture _defaultIcon;
        private static Texture _dummyFix;
        private static TrueTypeFont _defaultFont;

        internal static Texture LogoTexture => _logo ?? LazyLoad(ref _logo, LogoResourceKey);
        internal static Texture BetaEmblemTexture => _betaEmblem ?? LazyLoad(ref _betaEmblem, BetaEmblemResourceKey);
        internal static Texture DefaultIconTexture => _defaultIcon ?? LazyLoad(ref _defaultIcon, DefaultIconResourceKey);
        internal static Texture DummyFixTexture => _dummyFix ?? LazyLoad(ref _dummyFix, DummyFixResourceKey);
        internal static TrueTypeFont DefaultFont => _defaultFont ?? LazyLoad(ref _defaultFont, DefaultFontResourceKey, 16, null);

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