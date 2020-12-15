using System;
using System.Runtime.InteropServices;

namespace Chroma.Natives.SDL
{
    internal class SDL2_sound
    {
        internal const string LibraryName = "SDL2_sound";

        internal enum Sound_SampleFlags
        {
            SOUND_SAMPLEFLAG_NONE = 0,
            SOUND_SAMPLEFLAG_CANSEEK = 1,
            SOUND_SAMPLEFLAG_EOF = 1 << 29,
            SOUND_SAMPLEFLAG_ERROR = 1 << 30,
            SOUND_SAMPLEFLAG_EAGAIN = 1 << 30
        }

        [StructLayout(LayoutKind.Sequential)]
        internal struct Sound_AudioInfo
        {
            internal ushort format;
            internal byte channels;
            internal uint rate;
        }

        [StructLayout(LayoutKind.Sequential)]
        internal struct Sound_DecoderInfo
        {
            internal IntPtr extensions;
            internal IntPtr description;
            internal IntPtr author;
            internal IntPtr url;
        }

        [StructLayout(LayoutKind.Sequential)]
        internal struct Sound_Sample
        {
            internal IntPtr opaque;
            internal IntPtr decoder; // Sound_DecoderInfo*
            internal Sound_AudioInfo desired;
            internal Sound_AudioInfo actual;
            internal IntPtr buffer;
            internal uint buffer_size;
            internal Sound_SampleFlags flags;
        }

        [StructLayout(LayoutKind.Sequential)]
        internal struct Sound_Version
        {
            internal int major;
            internal int minor;
            internal int patch;
        }

        [DllImport(LibraryName, CallingConvention = CallingConvention.Cdecl)]
        internal static extern void Sound_GetLinkedVersion(out Sound_Version ver);

        [DllImport(LibraryName, CallingConvention = CallingConvention.Cdecl)]
        internal static extern int Sound_Init();

        [DllImport(LibraryName, CallingConvention = CallingConvention.Cdecl)]
        internal static extern int Sound_Quit();

        [DllImport(LibraryName, CallingConvention = CallingConvention.Cdecl, EntryPoint = "Sound_AvailableDecoders")]
        internal static extern IntPtr Sound_AvailableDecoders();

        [DllImport(LibraryName, CallingConvention = CallingConvention.Cdecl, EntryPoint = "Sound_GetError")]
        private static extern IntPtr Sound_GetError_INTERNAL();

        internal static string Sound_GetError()
        {
            return Marshal.PtrToStringAnsi(
                Sound_GetError_INTERNAL()
            );
        }
        
        [DllImport(LibraryName, CallingConvention = CallingConvention.Cdecl)]
        internal static extern void Sound_ClearError();

        [DllImport(LibraryName, CallingConvention = CallingConvention.Cdecl)]
        internal static extern IntPtr Sound_NewSample(
            IntPtr rw,
            [In, MarshalAs(UnmanagedType.LPStr)] string ext,
            ref Sound_AudioInfo desired,
            uint bufferSize
        );

        [DllImport(LibraryName, CallingConvention = CallingConvention.Cdecl)]
        internal static extern IntPtr Sound_NewSampleFromMem(
            [In, MarshalAs(UnmanagedType.LPArray, ArraySubType = UnmanagedType.U1)]
            byte[] data,
            uint size,
            [In, MarshalAs(UnmanagedType.LPStr)] string ext,
            uint bufferSize
        );

        [DllImport(LibraryName, CallingConvention = CallingConvention.Cdecl)]
        internal static extern IntPtr Sound_NewSampleFromFile(
            [In, MarshalAs(UnmanagedType.LPStr)] string fname,
            ref Sound_AudioInfo desired,
            uint bufferSize
        );

        [DllImport(LibraryName, CallingConvention = CallingConvention.Cdecl)]
        internal static extern IntPtr Sound_FreeSample(IntPtr sample);
        
        [DllImport(LibraryName, CallingConvention = CallingConvention.Cdecl)]
        internal static extern int Sound_GetDuration(IntPtr sample);

        [DllImport(LibraryName, CallingConvention = CallingConvention.Cdecl)]
        internal static extern int Sound_SetBufferSize(IntPtr sample, uint new_size);

        [DllImport(LibraryName, CallingConvention = CallingConvention.Cdecl)]
        internal static extern int Sound_Decode(IntPtr sample);
        
        [DllImport(LibraryName, CallingConvention = CallingConvention.Cdecl)]
        internal static extern int Sound_DecodeAll(IntPtr sample);

        [DllImport(LibraryName, CallingConvention = CallingConvention.Cdecl)]
        internal static extern int Sound_Rewind(IntPtr sample);

        [DllImport(LibraryName, CallingConvention = CallingConvention.Cdecl)]
        internal static extern int Sound_Seek(IntPtr sample, uint ms);
    }
}