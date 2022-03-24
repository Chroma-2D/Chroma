using Chroma.Natives.Bindings.FreeType.Native;

namespace Chroma.Graphics.TextRendering.TrueType
{
    public enum KerningMode
    {
        Default = FT_Kerning_Mode.FT_KERNING_DEFAULT,
        Unfitted = FT_Kerning_Mode.FT_KERNING_UNFITTED,
        Unscaled = FT_Kerning_Mode.FT_KERNING_UNSCALED
    }
}