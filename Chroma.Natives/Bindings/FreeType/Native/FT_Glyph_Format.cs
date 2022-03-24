namespace Chroma.Natives.Bindings.FreeType.Native
{
    internal enum FT_Glyph_Format : uint
    {
        FT_GLYPH_FORMAT_NONE = 0,

        FT_GLYPH_FORMAT_COMPOSITE = ((uint)'c' << 24) | ((uint)'o' << 16) | ((uint)'m' << 8) | 'p',
        FT_GLYPH_FORMAT_BITMAP = ((uint)'b' << 24) | ((uint)'i' << 16) | ((uint)'t' << 8) | 's',
        FT_GLYPH_FORMAT_OUTLINE = ((uint)'o' << 24) | ((uint)'u' << 16) | ((uint)'t' << 8) | 'l',
        FT_GLYPH_FORMAT_PLOTTER = ((uint)'p' << 24) | ((uint)'l' << 16) | ((uint)'o' << 8) | 't',
    }
}