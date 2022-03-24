using System;

namespace Chroma.Natives.Bindings.FreeType.Native
{
    internal delegate FT_Error FTC_Face_Requester(IntPtr faceId, IntPtr library, IntPtr requestData, out IntPtr aface);
}