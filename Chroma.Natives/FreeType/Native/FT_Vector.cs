﻿using System;
using System.Runtime.InteropServices;

namespace Chroma.Natives.FreeType.Native
{
    [StructLayout(LayoutKind.Sequential)]
    internal unsafe struct FT_Vector
    {
        public IntPtr x;
        public IntPtr y;
    }
}
