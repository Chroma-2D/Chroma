﻿namespace Chroma.Natives.FreeType.Native
{
    internal enum FT_Stroker_LineJoin
    {
        FT_STROKER_LINEJOIN_ROUND = 0,
        FT_STROKER_LINEJOIN_BEVEL = 1,
        FT_STROKER_LINEJOIN_MITER_VARIABLE = 2,
        FT_STROKER_LINEJOIN_MITER = FT_STROKER_LINEJOIN_MITER_VARIABLE,
        FT_STROKER_LINEJOIN_MITER_FIXED = 3
    }
}