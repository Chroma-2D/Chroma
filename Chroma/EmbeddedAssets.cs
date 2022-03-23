using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text.Json;
using System.Text.Json.Serialization;
using Chroma.Graphics;
using Chroma.Graphics.TextRendering.TrueType;

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

        private static Texture _logoTexture;
        private static Texture _betaEmblem;
        private static Texture _defaultIcon;
        private static Texture _dummyFix;
        private static TrueTypeFont _defaultFont;

        private static Texture _bootLogoTexture;
        private static Texture _bootTextTexture;
        private static Texture _bootWheelTexture;
        private static List<List<BootAnimationInfo>> _bootAnimData;

        internal static Texture DefaultIconTexture => Retrieve(ref _defaultIcon, nameof(_defaultIcon));
        internal static Texture DummyFixTexture => Retrieve(ref _dummyFix, nameof(_dummyFix));
        internal static TrueTypeFont DefaultFont => Retrieve(ref _defaultFont, nameof(_defaultFont));
        internal static Texture LogoTexture => Retrieve(ref _logoTexture, nameof(_logoTexture));
        internal static Texture BetaEmblemTexture => Retrieve(ref _betaEmblem, nameof(_betaEmblem));

        internal static Texture BootLogoTexture => Retrieve(ref _bootLogoTexture, nameof(_bootLogoTexture));
        internal static Texture BootTextTexture => Retrieve(ref _bootTextTexture, nameof(_bootTextTexture));
        internal static Texture BootWheelTexture => Retrieve(ref _bootWheelTexture, nameof(_bootWheelTexture));

        internal static List<List<BootAnimationInfo>> BootAnimationData =>
            _bootAnimData ?? LazyDeserialize(ref _bootAnimData, BootDataResourceKey);

        internal static void LoadEmbeddedAssets()
        {
            _defaultIcon = Load(ref _defaultIcon, DefaultIconResourceKey);
            _dummyFix = Load(ref _dummyFix, DummyFixResourceKey);
            _defaultFont = Load(ref _defaultFont, DefaultFontResourceKey, 16, null);
            _logoTexture = Load(ref _logoTexture, LogoResourceKey);
            _betaEmblem = Load(ref _betaEmblem, BetaEmblemResourceKey);
            _bootLogoTexture = Load(ref _bootLogoTexture, BootLogoResourceKey);
            _bootTextTexture = Load(ref _bootTextTexture, BootTextResourceKey);
            _bootWheelTexture = Load(ref _bootWheelTexture, BootWheelResourceKey);
        }

        private static T Retrieve<T>(ref T field, string name)
        {
            return field ?? throw new FrameworkException($"Internal failure: {name} not loaded");
        }

        private static T Load<T>(ref T field, string resourceKey, params object[] args)
        {
            if (field == null)
            {
                using var resourceStream = Assembly.GetExecutingAssembly()
                    .GetManifestResourceStream(resourceKey);

                var actualArgs = new[] { resourceStream }.Concat(args).ToArray();
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