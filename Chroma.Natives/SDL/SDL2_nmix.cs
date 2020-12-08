using System;
using System.Runtime.InteropServices;

namespace Chroma.Natives.SDL
{
    internal static class SDL2_nmix
    {
        internal const string LibraryName = "SDL2_nmix";

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        internal delegate void NMIX_SourceCallback(IntPtr userdata, IntPtr stream, int stream_size);

        [StructLayout(LayoutKind.Sequential)]
        internal struct NMIX_Source
        {
            internal int rate;
            internal ushort format; // SDL_AudioFormat
            internal byte channels;
            internal float pan;
            internal float gain;
            internal IntPtr callback; // NMIX_SourceCallback*
            internal IntPtr userdata; // void*
            internal int eof; // SDL_bool
            internal IntPtr in_buffer; // void*
            internal int in_buffer_size;
            internal IntPtr stream; // SDL_AudioStream*
            internal IntPtr out_buffer; // void*
            internal int out_buffer_size;
            internal IntPtr prev;
            internal IntPtr next;
        }

        [StructLayout(LayoutKind.Sequential)]
        internal struct NMIX_FileSource
        {
            internal IntPtr rw;  // SDL_RWops*
            internal IntPtr ext; // const char*
            internal IntPtr sample; //Sound_Sample
            internal IntPtr source; //NMIX_Source
            internal int loop_on; // SDL_bool
            internal IntPtr buffer; // Uint8*
            internal int bytes_left;
            internal int predecoded; // SDL_bool
        }

        [DllImport(LibraryName, CallingConvention = CallingConvention.Cdecl)]
        internal static extern int NMIX_OpenAudio(
            [In, MarshalAs(UnmanagedType.LPUTF8Str)]
            string device,
            int rate,
            int samples
        );

        [DllImport(LibraryName, CallingConvention = CallingConvention.Cdecl)]
        internal static extern int NMIX_CloseAudio();

        [DllImport(LibraryName, CallingConvention = CallingConvention.Cdecl)]
        internal static extern void NMIX_PausePlayback(bool pause_on);

        [DllImport(LibraryName, CallingConvention = CallingConvention.Cdecl)]
        internal static extern float NMIX_GetMasterGain();

        [DllImport(LibraryName, CallingConvention = CallingConvention.Cdecl)]
        internal static extern void NMIX_SetMasterGain(float gain);

        // SDL_AudioSpec
        [DllImport(LibraryName, CallingConvention = CallingConvention.Cdecl)]
        internal static extern IntPtr NMIX_GetAudioSpec();

        [DllImport(LibraryName, CallingConvention = CallingConvention.Cdecl)]
        internal static extern uint NMIX_GetAudioDevice();

        [DllImport(LibraryName, CallingConvention = CallingConvention.Cdecl)]
        internal static extern IntPtr NMIX_NewSource(
            ushort format, // SDL_AudioFormat
            byte channels,
            int rate,
            NMIX_SourceCallback callback,
            IntPtr userdata
        );

        [DllImport(LibraryName, CallingConvention = CallingConvention.Cdecl)]
        internal static extern void NMIX_FreeSource(IntPtr source);

        [DllImport(LibraryName, CallingConvention = CallingConvention.Cdecl)]
        internal static extern int NMIX_Play(IntPtr source);

        [DllImport(LibraryName, CallingConvention = CallingConvention.Cdecl)]
        internal static extern int NMIX_Pause(IntPtr source);

        [DllImport(LibraryName, CallingConvention = CallingConvention.Cdecl)]
        internal static extern bool NMIX_IsPlaying(IntPtr source);

        [DllImport(LibraryName, CallingConvention = CallingConvention.Cdecl)]
        internal static extern float NMIX_GetPan(IntPtr source);

        [DllImport(LibraryName, CallingConvention = CallingConvention.Cdecl)]
        internal static extern void NMIX_SetPan(IntPtr source, float pan);

        [DllImport(LibraryName, CallingConvention = CallingConvention.Cdecl)]
        internal static extern float NMIX_GetGain(IntPtr source);

        [DllImport(LibraryName, CallingConvention = CallingConvention.Cdecl)]
        internal static extern float NMIX_SetGain(IntPtr source, float gain);
        
        // NMIX_FileSource*
        [DllImport(LibraryName, CallingConvention = CallingConvention.Cdecl)]
        internal static extern IntPtr NMIX_NewFileSource(
            IntPtr rw,
            [In, MarshalAs(UnmanagedType.LPStr)] string ext,
            bool predecode
        );

        [DllImport(LibraryName, CallingConvention = CallingConvention.Cdecl)]
        internal static extern void NMIX_FreeFileSource(IntPtr s);

        [DllImport(LibraryName, CallingConvention = CallingConvention.Cdecl)]
        internal static extern int NMIX_GetDuration(IntPtr s);

        [DllImport(LibraryName, CallingConvention = CallingConvention.Cdecl)]
        internal static extern int NMIX_Seek(IntPtr s, int ms);

        [DllImport(LibraryName, CallingConvention = CallingConvention.Cdecl)]
        internal static extern int NMIX_Rewind(IntPtr s);

        [DllImport(LibraryName, CallingConvention = CallingConvention.Cdecl)]
        internal static extern bool NMIX_GetLoop(IntPtr s);

        [DllImport(LibraryName, CallingConvention = CallingConvention.Cdecl)]
        internal static extern void NMIX_SetLoop(IntPtr s, bool loop);
    }
}