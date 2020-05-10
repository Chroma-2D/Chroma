using System;
using System.Runtime.InteropServices;

namespace Chroma.Natives.FreeType.Native
{
    [StructLayout(LayoutKind.Sequential)]
    internal unsafe struct FT_CharMapRec
    {
        public FT_FaceRec* face;
        public FT_Encoding encoding;
        public ushort platform_id;
        public ushort encoding_id;
    }
}
