using System;
using System.Runtime.InteropServices;

namespace Chroma.Natives.OpenAL
{
    internal class Al
    {
        internal const string OpenAlLibName = "mojoal";

        internal const int AL_SOURCE_RELATIVE = 0x202;
        
        internal const int AL_CONE_INNER_ANGLE = 0x1001;
        internal const int AL_CONE_OUTER_ANGLE = 0x1002;
        internal const int AL_PITCH = 0x1003;
        internal const int AL_POSITION = 0x1004;
        internal const int AL_DIRECTION = 0x1005;
        internal const int AL_VELOCITY = 0x1006;
        internal const int AL_LOOPING = 0x1007;
        internal const int AL_BUFFER = 0x1009;
        internal const int AL_GAIN = 0x100A;
        internal const int AL_MIN_GAIN = 0x100D;
        internal const int AL_MAX_GAIN = 0x100E;
        internal const int AL_ORIENTATION = 0x100F;
        internal const int AL_SOURCE_STATE = 0x1010;
        internal const int AL_INITIAL = 0x1011;
        internal const int AL_PLAYING = 0x1012;
        internal const int AL_PAUSED = 0x1013;
        internal const int AL_STOPPED = 0x1014;
        internal const int AL_BUFFERS_QUEUED = 0x1015;
        internal const int AL_BUFFERS_PROCESSED = 0x1016;
        
        internal const int AL_REFERENCE_DISTANCE = 0x1020;
        internal const int AL_ROLLOFF_FACTOR = 0x1021;
        internal const int AL_CONE_OUTER_GAIN = 0x1022;
        internal const int AL_MAX_DISTANCE = 0x1023;
        internal const int AL_SEC_OFFSET = 0x1024;
        internal const int AL_SAMPLE_OFFSET = 0x1025;
        internal const int AL_BYTE_OFFSET = 0x1026;
        
        internal const int AL_SOURCE_TYPE = 0x1027;
        internal const int AL_STATIC = 0x1028;
        internal const int AL_STREAMING = 0x1029;
        internal const int AL_UNDETERMINED = 0x1030;
        
        internal const int AL_FORMAT_MONO8 = 0x1100;
        internal const int AL_FORMAT_MONO16 = 0x1101;
        internal const int AL_FORMAT_STEREO8 = 0x1102;
        internal const int AL_FORMAT_STEREO16 = 0x1103;

        internal const int AL_FREQUENCY = 0x2001;
        internal const int AL_BITS = 0x2002;
        internal const int AL_CHANNELS = 0x2003;
        internal const int AL_SIZE = 0x2004;

        internal const int AL_NO_ERROR = 0;
        internal const int AL_INVALID_NAME = 0xA001;
        internal const int AL_INVALID_ENUM = 0xA002;
        internal const int AL_INVALID_VALUE = 0xA003;
        internal const int AL_INVALID_OPERATION = 0xA004;
        internal const int AL_OUT_OF_MEMORY = 0xA005;

        internal const int AL_VENDOR = 0xB001;
        internal const int AL_VERSION = 0xB002;
        internal const int AL_RENDERER = 0xB003;
        internal const int AL_EXTENSIONS = 0xB004;

        internal const int AL_DOPPLER_FACTOR = 0xC000;
        internal const int AL_DOPPLER_VELOCITY = 0xC001;
        internal const int AL_SPEED_OF_SOUND = 0xC003;

        internal const int AL_DISTANCE_MODEL = 0xD000;
        internal const int AL_INVERSE_DISTANCE = 0xD001;
        internal const int AL_INVERSE_DISTANCE_CLAMPED = 0xD002;
        internal const int AL_LINEAR_DISTANCE = 0xD003;
        internal const int AL_LINEAR_DISTANCE_CLAMPED = 0xD004;
        internal const int AL_EXPONENT_DISTANCE = 0xD005;
        internal const int AL_EXPONENT_DISTANCE_CLAMPED = 0xD006;

        [DllImport(OpenAlLibName, CallingConvention = CallingConvention.Cdecl)]
        internal static extern void alDopplerFactor(float value);

        [DllImport(OpenAlLibName, CallingConvention = CallingConvention.Cdecl)]
        internal static extern void alDopplerVelocity(float value);

        [DllImport(OpenAlLibName, CallingConvention = CallingConvention.Cdecl)]
        internal static extern void alSpeedOfSound(float value);

        [DllImport(OpenAlLibName, CallingConvention = CallingConvention.Cdecl)]
        internal static extern void alDistanceModel(int distanceModel);

        [DllImport(OpenAlLibName, CallingConvention = CallingConvention.Cdecl)]
        internal static extern void alEnable(int capability);

        [DllImport(OpenAlLibName, CallingConvention = CallingConvention.Cdecl)]
        internal static extern void alDisable(int capability);

        [DllImport(OpenAlLibName, CallingConvention = CallingConvention.Cdecl)]
        internal static extern bool alIsEnabled(int capability);

        [DllImport(OpenAlLibName, CallingConvention = CallingConvention.Cdecl, EntryPoint = "alGetString")]
        private static extern IntPtr alGetString_INTERNAL(int param);

        internal static string alGetString(int param)
        {
            var strPtr = alGetString_INTERNAL(param);

            if (strPtr == IntPtr.Zero)
                return string.Empty;

            return Marshal.PtrToStringAnsi(strPtr);
        }

        [DllImport(OpenAlLibName, CallingConvention = CallingConvention.Cdecl)]
        internal static extern void alGetBooleanv(int param, bool[] values);

        [DllImport(OpenAlLibName, CallingConvention = CallingConvention.Cdecl)]
        internal static extern void alGetIntegerv(int param, int[] values);

        [DllImport(OpenAlLibName, CallingConvention = CallingConvention.Cdecl)]
        internal static extern void alGetFloatv(int param, float[] values);

        [DllImport(OpenAlLibName, CallingConvention = CallingConvention.Cdecl)]
        internal static extern void alGetDoublev(int param, double[] values);

        [DllImport(OpenAlLibName, CallingConvention = CallingConvention.Cdecl)]
        internal static extern bool alGetBoolean(int param);

        [DllImport(OpenAlLibName, CallingConvention = CallingConvention.Cdecl)]
        internal static extern int alGetInteger(int param);

        [DllImport(OpenAlLibName, CallingConvention = CallingConvention.Cdecl)]
        internal static extern float alGetFloat(int param);

        [DllImport(OpenAlLibName, CallingConvention = CallingConvention.Cdecl)]
        internal static extern float alGetDouble(int param);

        [DllImport(OpenAlLibName, CallingConvention = CallingConvention.Cdecl)]
        internal static extern int alGetError();

        [DllImport(OpenAlLibName, CallingConvention = CallingConvention.Cdecl)]
        internal static extern bool alIsExtensionPresent(
            [In, MarshalAs(UnmanagedType.LPStr)] string extname
        );

        [DllImport(OpenAlLibName, CallingConvention = CallingConvention.Cdecl)]
        internal static extern IntPtr alGetProcAddress(
            [In, MarshalAs(UnmanagedType.LPStr)] string fname
        );

        [DllImport(OpenAlLibName, CallingConvention = CallingConvention.Cdecl)]
        internal static extern int alGetEnumValue(
            [In, MarshalAs(UnmanagedType.LPStr)] string ename
        );

        [DllImport(OpenAlLibName, CallingConvention = CallingConvention.Cdecl)]
        internal static extern void alListenerf(int param, float value);

        [DllImport(OpenAlLibName, CallingConvention = CallingConvention.Cdecl)]
        internal static extern void alListener3f(int param, float v1, float v2, float v3);

        [DllImport(OpenAlLibName, CallingConvention = CallingConvention.Cdecl)]
        internal static extern void alListenerfv(int param, float[] values);

        [DllImport(OpenAlLibName, CallingConvention = CallingConvention.Cdecl)]
        internal static extern void alListeneri(int param, int value);

        [DllImport(OpenAlLibName, CallingConvention = CallingConvention.Cdecl)]
        internal static extern void alListener3i(int param, int v1, int v2, int v3);

        [DllImport(OpenAlLibName, CallingConvention = CallingConvention.Cdecl)]
        internal static extern void alListeneriv(int param, int[] values);

        [DllImport(OpenAlLibName, CallingConvention = CallingConvention.Cdecl)]
        internal static extern void alGetListenerf(int param, out float value);

        [DllImport(OpenAlLibName, CallingConvention = CallingConvention.Cdecl)]
        internal static extern void alGetListener3f(int param, out float v1, out float v2, out float v3);

        [DllImport(OpenAlLibName, CallingConvention = CallingConvention.Cdecl)]
        internal static extern void alGetListenerfv(int param, float[] values);
        
        [DllImport(OpenAlLibName, CallingConvention = CallingConvention.Cdecl)]
        internal static extern void alGetListeneri(int param, out int value);

        [DllImport(OpenAlLibName, CallingConvention = CallingConvention.Cdecl)]
        internal static extern void alGetListener3i(int param, out int v1, out int v2, out int v3);

        [DllImport(OpenAlLibName, CallingConvention = CallingConvention.Cdecl)]
        internal static extern void alGetListeneriv(int param, int[] values);

        [DllImport(OpenAlLibName, CallingConvention = CallingConvention.Cdecl)]
        internal static extern void alGenSources(int n, uint[] sources);

        [DllImport(OpenAlLibName, CallingConvention = CallingConvention.Cdecl)]
        internal static extern void alDeleteSources(int n, uint[] sources);

        [DllImport(OpenAlLibName, CallingConvention = CallingConvention.Cdecl)]
        internal static extern bool alIsSource(uint source);

        [DllImport(OpenAlLibName, CallingConvention = CallingConvention.Cdecl)]
        internal static extern void alSourcef(uint source, int param, float value);

        [DllImport(OpenAlLibName, CallingConvention = CallingConvention.Cdecl)]
        internal static extern void alSource3f(uint source, int param, float v1, float v2, float v3);

        [DllImport(OpenAlLibName, CallingConvention = CallingConvention.Cdecl)]
        internal static extern void alSourcefv(uint source, int param, float[] values);

        [DllImport(OpenAlLibName, CallingConvention = CallingConvention.Cdecl)]
        internal static extern void alSourcei(uint source, int param, int value);

        [DllImport(OpenAlLibName, CallingConvention = CallingConvention.Cdecl)]
        internal static extern void alSource3i(uint source, int param, int v1, int v2, int v3);

        [DllImport(OpenAlLibName, CallingConvention = CallingConvention.Cdecl)]
        internal static extern void alSourceiv(uint source, int param, int[] values);

        [DllImport(OpenAlLibName, CallingConvention = CallingConvention.Cdecl)]
        internal static extern void alGetSourcef(uint source, int param, out float value);

        [DllImport(OpenAlLibName, CallingConvention = CallingConvention.Cdecl)]
        internal static extern void alGetSource3f(uint source, int param, out float v1, out float v2, out float v3);

        [DllImport(OpenAlLibName, CallingConvention = CallingConvention.Cdecl)]
        internal static extern void alGetSourcefv(uint source, int param, float[] values);
        
        [DllImport(OpenAlLibName, CallingConvention = CallingConvention.Cdecl)]
        internal static extern void alGetSourcei(uint source, int param, out int value);

        [DllImport(OpenAlLibName, CallingConvention = CallingConvention.Cdecl)]
        internal static extern void alGetSource3i(uint source, int param, out int v1, out int v2, out int v3);

        [DllImport(OpenAlLibName, CallingConvention = CallingConvention.Cdecl)]
        internal static extern void alGetSourceiv(uint source, int param, int[] values);

        [DllImport(OpenAlLibName, CallingConvention = CallingConvention.Cdecl)]
        internal static extern void alSourcePlayv(int n, uint[] sources);

        [DllImport(OpenAlLibName, CallingConvention = CallingConvention.Cdecl)]
        internal static extern void alSourceStopv(int n, uint[] sources);

        [DllImport(OpenAlLibName, CallingConvention = CallingConvention.Cdecl)]
        internal static extern void alSourceRewindv(int n, uint[] sources);

        [DllImport(OpenAlLibName, CallingConvention = CallingConvention.Cdecl)]
        internal static extern void alSourcePausev(int n, uint[] sources);

        [DllImport(OpenAlLibName, CallingConvention = CallingConvention.Cdecl)]
        internal static extern void alSourcePlay(uint source);

        [DllImport(OpenAlLibName, CallingConvention = CallingConvention.Cdecl)]
        internal static extern void alSourceStop(uint source);

        [DllImport(OpenAlLibName, CallingConvention = CallingConvention.Cdecl)]
        internal static extern void alSourceRewind(uint source);

        [DllImport(OpenAlLibName, CallingConvention = CallingConvention.Cdecl)]
        internal static extern void alSourcePause(uint source);

        [DllImport(OpenAlLibName, CallingConvention = CallingConvention.Cdecl)]
        internal static extern void alSourceQueueBuffers(uint source, int nb, uint[] buffers);

        [DllImport(OpenAlLibName, CallingConvention = CallingConvention.Cdecl)]
        internal static extern void alSourceUnqueueBuffers(uint source, int nb, uint[] buffers);

        [DllImport(OpenAlLibName, CallingConvention = CallingConvention.Cdecl)]
        internal static extern void alGenBuffers(int n, uint[] buffers);

        [DllImport(OpenAlLibName, CallingConvention = CallingConvention.Cdecl)]
        internal static extern void alDeleteBuffers(int n, uint[] buffers);

        [DllImport(OpenAlLibName, CallingConvention = CallingConvention.Cdecl)]
        internal static extern bool alIsBuffer(uint buffer);

        [DllImport(OpenAlLibName, CallingConvention = CallingConvention.Cdecl)]
        internal static extern void alBufferData(uint buffer, int format, IntPtr data, int size, int freq);

        [DllImport(OpenAlLibName, CallingConvention = CallingConvention.Cdecl)]
        internal static extern void alBufferf(uint buffer, int param, float value);

        [DllImport(OpenAlLibName, CallingConvention = CallingConvention.Cdecl)]
        internal static extern void alBuffer3f(uint buffer, int param, float v1, float v2, float v3);

        [DllImport(OpenAlLibName, CallingConvention = CallingConvention.Cdecl)]
        internal static extern void alBufferfv(uint buffer, int param, float[] values);
        
        [DllImport(OpenAlLibName, CallingConvention = CallingConvention.Cdecl)]
        internal static extern void alBufferi(uint buffer, int param, int value);

        [DllImport(OpenAlLibName, CallingConvention = CallingConvention.Cdecl)]
        internal static extern void alBuffer3i(uint buffer, int param, int v1, int v2, int v3);

        [DllImport(OpenAlLibName, CallingConvention = CallingConvention.Cdecl)]
        internal static extern void alBufferiv(uint buffer, int param, int[] values);

        [DllImport(OpenAlLibName, CallingConvention = CallingConvention.Cdecl)]
        internal static extern void alGetBufferf(uint buffer, int param, out float value);

        [DllImport(OpenAlLibName, CallingConvention = CallingConvention.Cdecl)]
        internal static extern void alGetBuffer3f(uint buffer, int param, out float v1, out float v2, out float v3);

        [DllImport(OpenAlLibName, CallingConvention = CallingConvention.Cdecl)]
        internal static extern void alGetBufferfv(uint buffer, float[] values);
        
        [DllImport(OpenAlLibName, CallingConvention = CallingConvention.Cdecl)]
        internal static extern void alGetBufferi(uint buffer, int param, out int value);

        [DllImport(OpenAlLibName, CallingConvention = CallingConvention.Cdecl)]
        internal static extern void alGetBuffer3i(uint buffer, int param, out int v1, out int v2, out int v3);

        [DllImport(OpenAlLibName, CallingConvention = CallingConvention.Cdecl)]
        internal static extern void alGetBufferiv(uint buffer, int[] values);
    }
}