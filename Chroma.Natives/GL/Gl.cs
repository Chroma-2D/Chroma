using System.Runtime.InteropServices;
using Chroma.Natives.SDL;

namespace Chroma.Natives.GL
{
    public static class Gl
    {
        public const uint GL_MULTISAMPLE = 0x809D;
        public const uint GL_MAX_SAMPLES = 0x8D57;

        public const uint GL_NO_ERROR = 0;
        public const uint GL_INVALID_ENUM = 0x500;
        public const uint GL_INVALID_VALUE = 0x501;
        public const uint GL_INVALID_OPERATION = 0x502;
        public const uint GL_STACK_OVERFLOW = 0x503;
        public const uint GL_STACK_UNDERFLOW = 0x504;
        public const uint GL_OUT_OF_MEMORY = 0x505;
        
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        internal delegate void GlEnableDisableDelegate(uint extension);

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        internal delegate void GlGetIntegervDelegate(uint attr, out int result);

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        internal delegate uint GlGetErrorDelegate();
        
        internal static GlEnableDisableDelegate GlEnable { get; }
        internal static GlEnableDisableDelegate GlDisable { get; }
        internal static GlGetIntegervDelegate GlGetIntegerV { get; }
        internal static GlGetErrorDelegate GlGetError { get; }

        static Gl()
        {
            GlEnable = Marshal.GetDelegateForFunctionPointer<GlEnableDisableDelegate>(
                SDL2.SDL_GL_GetProcAddress("glEnable")
            );

            GlDisable = Marshal.GetDelegateForFunctionPointer<GlEnableDisableDelegate>(
                SDL2.SDL_GL_GetProcAddress("glDisable")
            );
            
            GlGetIntegerV = Marshal.GetDelegateForFunctionPointer<GlGetIntegervDelegate>(
                SDL2.SDL_GL_GetProcAddress("glGetIntegerv")
            );

            GlGetError = Marshal.GetDelegateForFunctionPointer<GlGetErrorDelegate>(
                SDL2.SDL_GL_GetProcAddress("glGetError")
            );
        }

        public static void SwitchMultiSampleAA(bool enabled)
        {
            if (enabled) GlEnable(GL_MULTISAMPLE);
            else GlDisable(GL_MULTISAMPLE);
        }
    }
}