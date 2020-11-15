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

        internal static string GetErrorMessage(int error)
        {
            return error switch
            {
                ALC_NO_ERROR => "there is not currently an error",
                ALC_INVALID_DEVICE => "a bad device was passed to an OpenAL function",
                ALC_INVALID_CONTEXT => "a bad context was passed to an OpenAL function",
                ALC_INVALID_ENUM => "an unknown enum value was passed to an OpenAL function",
                ALC_INVALID_VALUE => "an invalid value was passed to an OpenAL function",
                ALC_OUT_OF_MEMORY => "the requested operation resulted in OpenAL running out of memory",
                _ => "No message has been found for this error."
            };
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

        [DllImport(Al.OpenAlLibName, CallingConvention = CallingConvention.Cdecl, EntryPoint = "alcGetString")]
        internal static extern unsafe byte* alcGetString_UNSAFE(IntPtr device, int param);
        
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