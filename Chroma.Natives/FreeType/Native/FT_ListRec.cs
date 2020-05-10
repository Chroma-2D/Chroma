﻿using System;
using System.Runtime.InteropServices;

namespace Chroma.Natives.FreeType.Native
{
    [StructLayout(LayoutKind.Sequential)]
    internal unsafe struct FT_ListRec
    {
        public IntPtr head;
        public IntPtr tail;
    }
}
