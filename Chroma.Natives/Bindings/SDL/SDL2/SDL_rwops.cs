using System;
using System.Runtime.InteropServices;

namespace Chroma.Natives.Bindings.SDL
{
    internal static partial class SDL2
    {
        public const int RW_SEEK_SET = 0;
        public const int RW_SEEK_CUR = 1;
        public const int RW_SEEK_END = 2;

        public const UInt32 SDL_RWOPS_UNKNOWN = 0; /* Unknown stream type */
        public const UInt32 SDL_RWOPS_WINFILE = 1; /* Win32 file */
        public const UInt32 SDL_RWOPS_STDFILE = 2; /* Stdio file */
        public const UInt32 SDL_RWOPS_JNIFILE = 3; /* Android asset */
        public const UInt32 SDL_RWOPS_MEMORY = 4; /* Memory stream */
        public const UInt32 SDL_RWOPS_MEMORY_RO = 5; /* Read-Only memory stream */

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        public delegate long SDLRWopsSizeCallback(IntPtr context);

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        public delegate long SDLRWopsSeekCallback(
            IntPtr context,
            long offset,
            int whence
        );

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        public delegate IntPtr SDLRWopsReadCallback(
            IntPtr context,
            IntPtr ptr,
            IntPtr size,
            IntPtr maxnum
        );

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        public delegate IntPtr SDLRWopsWriteCallback(
            IntPtr context,
            IntPtr ptr,
            IntPtr size,
            IntPtr num
        );

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        public delegate int SDLRWopsCloseCallback(
            IntPtr context
        );

        [StructLayout(LayoutKind.Sequential)]
        public struct SDL_RWops
        {
            public IntPtr size;
            public IntPtr seek;
            public IntPtr read;
            public IntPtr write;
            public IntPtr close;

            public UInt32 type;

            /* NOTE: This isn't the full structure since
            * the native SDL_RWops contains a hidden union full of
            * internal information and platform-specific stuff depending
            * on what conditions the native library was built with
            */
        }

        [DllImport(LibraryName, EntryPoint = "SDL_RWFromFile", CallingConvention = CallingConvention.Cdecl)]
        private static extern unsafe IntPtr INTERNAL_SDL_RWFromFile(
            byte* file,
            byte* mode
        );

        public static unsafe IntPtr SDL_RWFromFile(
            string file,
            string mode
        )
        {
            byte* utf8File = Utf8EncodeHeap(file);
            byte* utf8Mode = Utf8EncodeHeap(mode);
            IntPtr rwOps = INTERNAL_SDL_RWFromFile(
                utf8File,
                utf8Mode
            );
            Marshal.FreeHGlobal((IntPtr)utf8Mode);
            Marshal.FreeHGlobal((IntPtr)utf8File);
            return rwOps;
        }

        [DllImport(LibraryName, CallingConvention = CallingConvention.Cdecl)]
        public static extern IntPtr SDL_AllocRW();

        [DllImport(LibraryName, CallingConvention = CallingConvention.Cdecl)]
        public static extern void SDL_FreeRW(IntPtr area);

        [DllImport(LibraryName, CallingConvention = CallingConvention.Cdecl)]
        public static extern IntPtr SDL_RWFromFP(IntPtr fp, bool autoclose);

        [DllImport(LibraryName, CallingConvention = CallingConvention.Cdecl)]
        public static extern IntPtr SDL_RWFromMem(IntPtr mem, int size);

        [DllImport(LibraryName, CallingConvention = CallingConvention.Cdecl)]
        public static extern IntPtr SDL_RWFromConstMem(IntPtr mem, int size);

        [DllImport(LibraryName, CallingConvention = CallingConvention.Cdecl)]
        public static extern long SDL_RWsize(IntPtr context);

        [DllImport(LibraryName, CallingConvention = CallingConvention.Cdecl)]
        public static extern long SDL_RWseek(
            IntPtr context,
            long offset,
            int whence
        );

        [DllImport(LibraryName, CallingConvention = CallingConvention.Cdecl)]
        public static extern long SDL_RWtell(IntPtr context);

        [DllImport(LibraryName, CallingConvention = CallingConvention.Cdecl)]
        public static extern long SDL_RWread(
            IntPtr context,
            IntPtr ptr,
            IntPtr size,
            IntPtr maxnum
        );

        [DllImport(LibraryName, CallingConvention = CallingConvention.Cdecl)]
        public static extern long SDL_RWwrite(
            IntPtr context,
            IntPtr ptr,
            IntPtr size,
            IntPtr maxnum
        );

        [DllImport(LibraryName, CallingConvention = CallingConvention.Cdecl)]
        public static extern byte SDL_ReadU8(IntPtr src);

        [DllImport(LibraryName, CallingConvention = CallingConvention.Cdecl)]
        public static extern UInt16 SDL_ReadLE16(IntPtr src);

        [DllImport(LibraryName, CallingConvention = CallingConvention.Cdecl)]
        public static extern UInt16 SDL_ReadBE16(IntPtr src);

        [DllImport(LibraryName, CallingConvention = CallingConvention.Cdecl)]
        public static extern UInt32 SDL_ReadLE32(IntPtr src);

        [DllImport(LibraryName, CallingConvention = CallingConvention.Cdecl)]
        public static extern UInt32 SDL_ReadBE32(IntPtr src);

        [DllImport(LibraryName, CallingConvention = CallingConvention.Cdecl)]
        public static extern UInt64 SDL_ReadLE64(IntPtr src);

        [DllImport(LibraryName, CallingConvention = CallingConvention.Cdecl)]
        public static extern UInt64 SDL_ReadBE64(IntPtr src);

        [DllImport(LibraryName, CallingConvention = CallingConvention.Cdecl)]
        public static extern uint SDL_WriteU8(IntPtr dst, byte value);

        [DllImport(LibraryName, CallingConvention = CallingConvention.Cdecl)]
        public static extern uint SDL_WriteLE16(IntPtr dst, UInt16 value);

        [DllImport(LibraryName, CallingConvention = CallingConvention.Cdecl)]
        public static extern uint SDL_WriteBE16(IntPtr dst, UInt16 value);

        [DllImport(LibraryName, CallingConvention = CallingConvention.Cdecl)]
        public static extern uint SDL_WriteLE32(IntPtr dst, UInt32 value);

        [DllImport(LibraryName, CallingConvention = CallingConvention.Cdecl)]
        public static extern uint SDL_WriteBE32(IntPtr dst, UInt32 value);

        [DllImport(LibraryName, CallingConvention = CallingConvention.Cdecl)]
        public static extern uint SDL_WriteLE64(IntPtr dst, UInt64 value);

        [DllImport(LibraryName, CallingConvention = CallingConvention.Cdecl)]
        public static extern uint SDL_WriteBE64(IntPtr dst, UInt64 value);

        [DllImport(LibraryName, CallingConvention = CallingConvention.Cdecl)]
        public static extern long SDL_RWclose(IntPtr context);

        [DllImport(LibraryName, EntryPoint = "SDL_LoadFile", CallingConvention = CallingConvention.Cdecl)]
        private static extern unsafe IntPtr INTERNAL_SDL_LoadFile(byte* file, out IntPtr datasize);

        public static unsafe IntPtr SDL_LoadFile(string file, out IntPtr datasize)
        {
            byte* utf8File = Utf8EncodeHeap(file);
            IntPtr result = INTERNAL_SDL_LoadFile(utf8File, out datasize);
            Marshal.FreeHGlobal((IntPtr)utf8File);
            return result;
        }
    }
}