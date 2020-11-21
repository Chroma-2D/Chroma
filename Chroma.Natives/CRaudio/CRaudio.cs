using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;

namespace Chroma.Natives.CRaudio
{
    internal class CRaudio
    {
        internal const string LibraryName = "craudio";
        
        internal const int CR_ERROR_NONE = 0;
        internal const int CR_ERROR_OUT_OF_MEMORY = 0xA000;
        internal const int CR_ERROR_FOPEN_FAILED = 0xA001;
        internal const int CR_ERROR_INVALID_POINTER = 0xA002;
        internal const int CR_ERROR_OGGVORBIS_STREAM_INVALID = 0xA010;
        internal const int CR_ERROR_OGGVORBIS_FILE_CORRUPT = 0xA011;

        internal delegate bool AudioLoaderDelegate(string path, out CRaudio_LoadInfo info);

        internal static List<AudioLoaderDelegate> AudioLoaders = new List<AudioLoaderDelegate>
        {
            CR_LoadOgg,
            CR_LoadTrackerModule
        };
        
        [StructLayout(LayoutKind.Sequential)]
        internal struct CRaudio_LoadInfo
        {
            public int format;
            public IntPtr data;
            public int size;
            public int freq;
        }

        [DllImport(LibraryName, CallingConvention = CallingConvention.Cdecl)]
        internal static extern bool CR_LoadOgg(
            [In, MarshalAs(UnmanagedType.LPStr)] string path,
            out CRaudio_LoadInfo info
        );

        [DllImport(LibraryName, CallingConvention = CallingConvention.Cdecl)]
        internal static extern bool CR_LoadTrackerModule(
            [In, MarshalAs(UnmanagedType.LPStr)] string path,
            out CRaudio_LoadInfo info
        );

        [DllImport(LibraryName, CallingConvention = CallingConvention.Cdecl)]
        internal static extern bool CR_Free([In] ref CRaudio_LoadInfo info);

        [DllImport(LibraryName, CallingConvention = CallingConvention.Cdecl)]
        internal static extern int CR_GetError();
    }
}