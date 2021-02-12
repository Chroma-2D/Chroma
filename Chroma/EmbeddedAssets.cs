using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text.Json;
using System.Text.Json.Serialization;
using Chroma.Graphics;
using Chroma.Graphics.TextRendering;

namespace Chroma
{
    internal class EmbeddedAssets
    {
        internal class BootAnimationInfo
        {
            [JsonPropertyName("o")]
            public float Opacity { get; set; }

            [JsonPropertyName("r")]
            public float Rotation { get; set; }

            [JsonPropertyName("s")]
            public float Scale { get; set; }
        }
        
        internal const string LogoResourceKey = "Chroma.Resources.logo.png";
        internal const string BetaEmblemResourceKey = "Chroma.Resources.beta.png";
        internal const string DummyFixResourceKey = "Chroma.Resources.dummy.png";
        internal const string DefaultIconResourceKey = "Chroma.Resources.deficon.png";
        internal const string DefaultFontResourceKey = "Chroma.Resources.default.ttf";
        
        internal const string BootDataResourceKey = "Chroma.Resources.boot.anim.json";
        internal const string BootLogoResourceKey = "Chroma.Resources.boot.crlogo.png";
        internal const string BootTextResourceKey = "Chroma.Resources.boot.crsub.png";
        internal const string BootWheelResourceKey = "Chroma.Resources.boot.crwheel.png";
        
        private static Texture _logo;
        private static Texture _betaEmblem;
        private static Texture _defaultIcon;
        private static Texture _dummyFix;
        private static TrueTypeFont _defaultFont;

        private static Texture _bootLogo;
        private static Texture _bootText;
        private static Texture _bootWheel;
        private static List<List<BootAnimationInfo>> _bootAnimData;

        internal static Texture LogoTexture => _logo ?? LazyLoad(ref _logo, LogoResourceKey);
        internal static Texture BetaEmblemTexture => _betaEmblem ?? LazyLoad(ref _betaEmblem, BetaEmblemResourceKey);
        internal static Texture DefaultIconTexture => _defaultIcon ?? LazyLoad(ref _defaultIcon, DefaultIconResourceKey);
        internal static Texture DummyFixTexture => _dummyFix ?? LazyLoad(ref _dummyFix, DummyFixResourceKey);
        internal static TrueTypeFont DefaultFont => _defaultFont ?? LazyLoad(ref _defaultFont, DefaultFontResourceKey, 16, null);

        internal static Texture BootLogoTexture => _bootLogo ?? LazyLoad(ref _bootLogo, BootLogoResourceKey);
        internal static Texture BootTextTexture => _bootText ?? LazyLoad(ref _bootText, BootTextResourceKey);
        internal static Texture BootWheelTexture => _bootWheel ?? LazyLoad(ref _bootWheel, BootWheelResourceKey);
        internal static List<List<BootAnimationInfo>> BootAnimationData => _bootAnimData ?? LazyDeserialize(ref _bootAnimData, BootDataResourceKey);

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

        private static T LazyDeserialize<T>(ref T field, string resourceKey)
        {
            if (field == null)
            {
                using var resourceStream = Assembly.GetExecutingAssembly()
                    .GetManifestResourceStream(resourceKey);

                using var sr = new StreamReader(resourceStream!);
                
                try
                {
                    field = JsonSerializer.Deserialize<T>(sr.ReadToEnd());
                }
                catch
                {
                    // Remains null upon failure.
                }
            }

            return field;
        }
    }
}