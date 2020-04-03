﻿using System;
using System.Runtime.InteropServices;

namespace Chroma.Natives.FreeType.Native
{
    [StructLayout(LayoutKind.Sequential)]
    public unsafe struct FT_Outline
    {
        public short n_contour;
        public short n_points;

        public FT_Vector* points;
        public byte* tags;
        public short* contours;

        public int flags;
    }
}
