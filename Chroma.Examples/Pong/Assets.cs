namespace Pong;

using Chroma.Audio.Sfxr;
using Chroma.ContentManagement;
using Chroma.Graphics;
using Chroma.Graphics.TextRendering.TrueType;

public static class Assets
{
    public static Texture Stretchy { get; private set; }
    public static TrueTypeFont ScoreFont { get; private set; }

    public static SfxrWaveform WallHit { get; private set; }
    public static SfxrWaveform PaddleHit { get; private set; }
    public static SfxrWaveform OutsidePlayfield { get; private set; }

    public static void Load(IContentProvider content)
    {
        Stretchy = content.Load<Texture>("Sprites/stretchy.png");
        ScoreFont = content.Load<TrueTypeFont>("Fonts/visitor2.ttf", 120);
        ScoreFont.HintingMode = HintingMode.Monochrome;

        WallHit = content.Load<SfxrWaveform>("Sounds/wall.sfxr", ParameterFormat.Binary);
        PaddleHit = content.Load<SfxrWaveform>("Sounds/paddle.sfxr", ParameterFormat.Binary);
            
        OutsidePlayfield = content.Load<SfxrWaveform>("Sounds/outsidePlayfield.sfxr", ParameterFormat.Binary);
        OutsidePlayfield.Volume -= 0.3f;
    }
}