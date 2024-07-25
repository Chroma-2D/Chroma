namespace Chroma.Natives.Bindings.SDL;

using System.Runtime.InteropServices;

internal static partial class SDL2
{
    [StructLayout(LayoutKind.Sequential)]
    public struct SDL_version
    {
        public byte major;
        public byte minor;
        public byte patch;
    }
		
    [DllImport(LibraryName, CallingConvention = CallingConvention.Cdecl)]
    public static extern void SDL_GetVersion(out SDL_version ver);
}