using System;
using System.Runtime.InteropServices;

namespace Chroma.Natives.OpenAL
{
    internal class Alc
    {
        internal const int ALC_FREQUENCY = 0x1007;
        internal const int ALC_REFRESH = 0x1008;
        internal const int ALC_SYNC = 0x1009;
        internal const int ALC_MONO_SOURCES = 0x1010;
        internal const int ALC_STEREO_SOURCES = 0x1011;

        internal const int ALC_NO_ERROR = 0;
        internal const int ALC_INVALID_DEVICE = 0xA001;
        internal const int ALC_INVALID_CONTEXT = 0xA002;
        internal const int ALC_INVALID_ENUM = 0xA003;
        internal const int ALC_INVALID_VALUE = 0xA004;
        internal const int ALC_OUT_OF_MEMORY = 0xA005;

        internal const int ALC_MAJOR_VERSION = 0x1000;
        internal const int ALC_MINOR_VERSION = 0x1001;

        internal const int ALC_ATTRIBUTES_SIZE = 0x1002;
        internal const int ALC_ALL_ATTRIBUTES = 0x1003;

        internal const int ALC_DEFAULT_DEVICE_SPECIFIER = 0x1004;

        internal const int ALC_DEVICE_SPECIFIER = 0x1005;
        internal const int ALC_EXTENSIONS = 0x1006;

        internal const int ALC_EXT_CAPTURE = 1;

        internal const int ALC_CAPTURE_DEVICE_SPECIFIER = 0x310;
        internal const int ALC_CAPTURE_DEFAULT_DEVICE_SPECIFIER = 0x311;
        internal const int ALC_CAPTURE_SAMPLES = 0x312;

        internal const int ALC_ENUMERATE_ALL_EXT = 1;
        internal const int ALC_DEFAULT_ALL_DEVICES_SPECIFIER = 0x1012;
        internal const int ALC_ALL_DEVICES_SPECIFIER = 0x1013;

        [StructLayout(LayoutKind.Explicit, Pack = 16)]
        internal struct ALClistener
        {
            [FieldOffset(0)] public IntPtr position;
            [FieldOffset(16)] public IntPtr velocity;
            [FieldOffset(32)] public IntPtr orientation;
            [FieldOffset(64)] public float gain;
        }

        [StructLayout(LayoutKind.Sequential)]
        internal struct ALCcontext
        {
            public IntPtr source_blocks; // SourceBlock**
            public int num_source_blocks;

            public ALClistener listener; // SIMDALIGNEDSTRUCT

            public IntPtr device; // ALCdevice*
            public int processing; // SDL_atomic_t
            public int error;
            public IntPtr attributes; // ALCint*
            public int attributes_count;

            public bool recalc;
            public int distance_model;

            public float doppler_factor;
            public float doppler_velocity;
            public float speed_of_sound;

            public IntPtr source_lock; // SDL_mutex*

            public IntPtr playlist_todo; // void*
            public IntPtr playlist; // ALsource*
            public IntPtr playlist_tail; // ALsource*

            public IntPtr prev; // ALCcontext*
            public IntPtr next; // ALCcontext*
        }

        [StructLayout(LayoutKind.Sequential)]
        internal struct MojoALplayback
        {
            public IntPtr contexts; // ALCcontext*
            public IntPtr buffer_blocks; // BufferBlock**
            public int num_buffer_blocks;
            public IntPtr buffer_queue_pool; // BufferQueueItem*
            public IntPtr source_todo_pool; // void*
        }

        internal struct RingBuffer
        {
            public IntPtr buffer; // *ALCubyte
            public int size;
            public int write;
            public int read;
            public int used;
        }

        [StructLayout(LayoutKind.Sequential)]
        internal struct MojoALcapture
        {
            public RingBuffer ring;
        }

        [StructLayout(LayoutKind.Explicit)]
        internal struct ALCdevice
        {
            [FieldOffset(0)] public IntPtr name;
            [FieldOffset(4)] public int error;
            [FieldOffset(8)] public int connected;
            [FieldOffset(12)] public bool iscapture;
            [FieldOffset(13)] public uint sdldevice;
            [FieldOffset(17)] public int channels;
            [FieldOffset(21)] public int frequency;
            [FieldOffset(25)] public int framesize;
            [FieldOffset(29)] public MojoALplayback playback; // union
            [FieldOffset(29)] public MojoALcapture capture; // union
        }

        [DllImport(Al.OpenAlLibName, CallingConvention = CallingConvention.Cdecl)]
        internal static extern IntPtr alcCreateContext(IntPtr device, int[] attrlist);

        [DllImport(Al.OpenAlLibName, CallingConvention = CallingConvention.Cdecl)]
        internal static extern bool alcMakeContextCurrent(IntPtr context);

        [DllImport(Al.OpenAlLibName, CallingConvention = CallingConvention.Cdecl)]
        internal static extern void alcProcessContext(IntPtr context);

        [DllImport(Al.OpenAlLibName, CallingConvention = CallingConvention.Cdecl)]
        internal static extern void alcSuspendContext(IntPtr context);

        [DllImport(Al.OpenAlLibName, CallingConvention = CallingConvention.Cdecl)]
        internal static extern void alcDestroyContext(IntPtr context);

        [DllImport(Al.OpenAlLibName, CallingConvention = CallingConvention.Cdecl)]
        internal static extern IntPtr alcGetCurrentContext();

        [DllImport(Al.OpenAlLibName, CallingConvention = CallingConvention.Cdecl)]
        internal static extern IntPtr alcGetContextsDevice(IntPtr context);

        [DllImport(Al.OpenAlLibName, CallingConvention = CallingConvention.Cdecl)]
        internal static extern IntPtr alcOpenDevice(
            [In, MarshalAs(UnmanagedType.LPStr)] string devicename
        );

        [DllImport(Al.OpenAlLibName, CallingConvention = CallingConvention.Cdecl)]
        internal static extern bool alcCloseDevice(IntPtr device);

        [DllImport(Al.OpenAlLibName, CallingConvention = CallingConvention.Cdecl)]
        internal static extern int alcGetError(IntPtr device);

        [DllImport(Al.OpenAlLibName, CallingConvention = CallingConvention.Cdecl)]
        internal static extern bool alcIsExtensionPresent(
            IntPtr device,
            [In, MarshalAs(UnmanagedType.LPStr)] string extname
        );

        [DllImport(Al.OpenAlLibName, CallingConvention = CallingConvention.Cdecl)]
        internal static extern IntPtr alcGetProcAddress(
            IntPtr device,
            [In, MarshalAs(UnmanagedType.LPStr)] string funcname
        );

        [DllImport(Al.OpenAlLibName, CallingConvention = CallingConvention.Cdecl)]
        internal static extern int alcGetEnumValue(
            IntPtr device,
            [In, MarshalAs(UnmanagedType.LPStr)] string enumname
        );

        [DllImport(Al.OpenAlLibName, CallingConvention = CallingConvention.Cdecl, EntryPoint = "alcGetString")]
        private static extern IntPtr alcGetString_INTERNAL(IntPtr device, int param);

        internal static string alcGetString(IntPtr device, int param)
        {
            var strPtr = alcGetString_INTERNAL(device, param);

            if (strPtr == IntPtr.Zero)
                return string.Empty;

            return Marshal.PtrToStringAnsi(strPtr);
        }

        [DllImport(Al.OpenAlLibName, CallingConvention = CallingConvention.Cdecl)]
        internal static extern IntPtr alcGetIntegerv(IntPtr device, int param, int size, int[] values);

        [DllImport(Al.OpenAlLibName, CallingConvention = CallingConvention.Cdecl)]
        internal static extern IntPtr alcCaptureOpenDevice(
            [In, MarshalAs(UnmanagedType.LPStr)] string devicename,
            int frequency,
            int format,
            int buffersize
        );

        [DllImport(Al.OpenAlLibName, CallingConvention = CallingConvention.Cdecl)]
        internal static extern IntPtr alcCaptureCloseDevice(IntPtr device);

        [DllImport(Al.OpenAlLibName, CallingConvention = CallingConvention.Cdecl)]
        internal static extern void alcCaptureStart(IntPtr device);

        [DllImport(Al.OpenAlLibName, CallingConvention = CallingConvention.Cdecl)]
        internal static extern void alcCaptureStop(IntPtr device);

        [DllImport(Al.OpenAlLibName, CallingConvention = CallingConvention.Cdecl)]
        internal static extern void alcCaptureSamples(IntPtr device, IntPtr buffer, int samples);
    }
}