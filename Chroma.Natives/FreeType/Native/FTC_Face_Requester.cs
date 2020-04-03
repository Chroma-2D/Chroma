using System;

namespace Chroma.Natives.FreeType.Native
{
	public delegate FT_Error FTC_Face_Requester(IntPtr faceId, IntPtr library, IntPtr requestData, out IntPtr aface);
}