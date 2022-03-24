using Chroma.Natives.Bindings.FreeType;

namespace Chroma.Graphics.TextRendering.TrueType
{
    public enum KerningMode
    {
        Default = FT2.FT_Kerning_Mode.FT_KERNING_DEFAULT,
        Unfitted = FT2.FT_Kerning_Mode.FT_KERNING_UNFITTED,
        Unscaled = FT2.FT_Kerning_Mode.FT_KERNING_UNSCALED
    }
}