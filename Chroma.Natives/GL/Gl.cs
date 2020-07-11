using System.Runtime.InteropServices;
using Chroma.Natives.SDL;

namespace Chroma.Natives.GL
{
    public static class Gl
    {
        public const uint GL_LINE_SMOOTH = 0x0B20;
        public const uint GL_MULTISAMPLE = 0x809D;
        public const uint GL_MAX_SAMPLES = 0x8D57;

        public const uint GL_DONT_CARE = 0x1100;
        public const uint GL_FASTEST = 0x1101;
        public const uint GL_NICEST = 0x1102;

        public const uint GL_LINE_SMOOTH_HINT = 0x0C52;

        public const uint GL_NO_ERROR = 0;
        public const uint GL_INVALID_ENUM = 0x500;
        public const uint GL_INVALID_VALUE = 0x501;
        public const uint GL_INVALID_OPERATION = 0x502;
        public const uint GL_STACK_OVERFLOW = 0x503;
        public const uint GL_STACK_UNDERFLOW = 0x504;
        public const uint GL_OUT_OF_MEMORY = 0x505;

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        internal delegate void GlEnableDisableDelegate(uint cap);

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        internal delegate void GlGetIntegervDelegate(uint attr, out int result);

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        internal delegate uint GlGetErrorDelegate();

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        internal delegate bool GlIsEnabledDelegate(uint cap);

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        internal delegate void GlHintDelegate(uint target, uint mode);
        
        internal static GlEnableDisableDelegate Enable =>
            Marshal.GetDelegateForFunctionPointer<GlEnableDisableDelegate>(
                SDL2.SDL_GL_GetProcAddress("glEnable")
            );

        internal static GlEnableDisableDelegate Disable =>
            Marshal.GetDelegateForFunctionPointer<GlEnableDisableDelegate>(
                SDL2.SDL_GL_GetProcAddress("glDisable")
            );

        internal static GlGetIntegervDelegate GetIntegerV =>
            Marshal.GetDelegateForFunctionPointer<GlGetIntegervDelegate>(
                SDL2.SDL_GL_GetProcAddress("glGetIntegerv")
            );

        internal static GlGetErrorDelegate GetError => 
            Marshal.GetDelegateForFunctionPointer<GlGetErrorDelegate>(
                SDL2.SDL_GL_GetProcAddress("glGetError")
            );

        internal static GlIsEnabledDelegate IsEnabled =>
            Marshal.GetDelegateForFunctionPointer<GlIsEnabledDelegate>(
                SDL2.SDL_GL_GetProcAddress("glIsEnabled")
            );

        internal static GlHintDelegate Hint =>
            Marshal.GetDelegateForFunctionPointer<GlHintDelegate>(
                SDL2.SDL_GL_GetProcAddress("glHint")
            );
    }
}