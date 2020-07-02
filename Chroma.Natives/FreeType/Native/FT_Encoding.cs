namespace Chroma.Natives.FreeType.Native
{
    internal enum FT_Encoding : uint
    {
        FT_ENCODING_NONE = 0,

        FT_ENCODING_MS_SYMBOL = ((uint)'s' << 24) | ((uint)'y' << 16) | ((uint)'m' << 8) | 'b',
        FT_ENCODING_UNICODE = ((uint)'u' << 24) | ((uint)'n' << 16) | ((uint)'i' << 8) | 'c',

        FT_ENCODING_SJIS = ((uint)'s' << 24) | ((uint)'j' << 16) | ((uint)'i' << 8) | 's',
        FT_ENCODING_PRC = ((uint)'g' << 24) | ((uint)'b' << 16) | ((uint)' ' << 8) | ' ',
        FT_ENCODING_BIG5 = ((uint)'b' << 24) | ((uint)'i' << 16) | ((uint)'g' << 8) | '5',
        FT_ENCODING_WANSUNG = ((uint)'w' << 24) | ((uint)'a' << 16) | ((uint)'n' << 8) | 's',
        FT_ENCODING_JOHAB = ((uint)'j' << 24) | ((uint)'o' << 16) | ((uint)'h' << 8) | 'a',

        FT_ENCODING_GB2312 = FT_ENCODING_PRC,
        FT_ENCODING_MS_SJIS = FT_ENCODING_SJIS,
        FT_ENCODING_MS_GB2312 = FT_ENCODING_PRC,
        FT_ENCODING_MS_BIG5 = FT_ENCODING_BIG5,
        FT_ENCODING_MS_WANSUNG = FT_ENCODING_WANSUNG,
        FT_ENCODING_MS_JOHAB = FT_ENCODING_JOHAB,

        FT_ENCODING_ADOBE_STANDARD = ((uint)'A' << 24) | ((uint)'D' << 16) | ((uint)'O' << 8) | 'B',
        FT_ENCODING_ADOBE_EXPERT = ((uint)'A' << 24) | ((uint)'D' << 16) | ((uint)'B' << 8) | 'E',
        FT_ENCODING_ADOBE_CUSTOM = ((uint)'A' << 24) | ((uint)'D' << 16) | ((uint)'B' << 8) | 'C',
        FT_ENCODING_ADOBE_LATIN_1 = ((uint)'l' << 24) | ((uint)'a' << 16) | ((uint)'t' << 8) | '1',

        FT_ENCODING_OLD_LATIN_2 = ((uint)'l' << 24) | ((uint)'a' << 16) | ((uint)'t' << 8) | '2',

        FT_ENCODING_APPLE_ROMAN = ((uint)'a' << 24) | ((uint)'r' << 16) | ((uint)'m' << 8) | 'n',
    }
}