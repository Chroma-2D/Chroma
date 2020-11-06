using System;
using System.Runtime.InteropServices;
using Chroma.Natives.SDL;

namespace Chroma.Natives.GL
{
    internal static class Gl
    {
        internal const int GL_BLEND_DST_RGB = 0x80C8;
        internal const int GL_BLEND_SRC_RGB = 0x80C9;
        internal const int GL_BLEND_DST_ALPHA = 0x80CA;
        internal const int GL_BLEND_SRC_ALPHA = 0x80CB;
        internal const int GL_BLEND_EQUATION_RGB = 0x8009;
        internal const int GL_BLEND_EQUATION_ALPHA = 0x883D;

        internal const uint GL_LINE_SMOOTH = 0x0B20;
        internal const uint GL_LINE_WIDTH = 0x0B21;
        
        internal const uint GL_MULTISAMPLE = 0x809D;
        internal const uint GL_MAX_SAMPLES = 0x8D57;

        internal const uint GL_DONT_CARE = 0x1100;
        internal const uint GL_FASTEST = 0x1101;
        internal const uint GL_NICEST = 0x1102;

        internal const uint GL_LINE_SMOOTH_HINT = 0x0C52;

        internal const uint GL_NO_ERROR = 0;
        internal const uint GL_INVALID_ENUM = 0x500;
        internal const uint GL_INVALID_VALUE = 0x501; 
        internal const uint GL_INVALID_OPERATION = 0x502;
        internal const uint GL_STACK_OVERFLOW = 0x503;
        internal const uint GL_STACK_UNDERFLOW = 0x504;
        internal const uint GL_OUT_OF_MEMORY = 0x505;
        
        internal const uint GL_NUM_EXTENSIONS = 0x821D;
        internal const uint GL_EXTENSIONS = 0x1F03;

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        internal delegate void GlEnableDisableDelegate(uint cap);

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        internal delegate void GlGetIntegervDelegate(uint attr, out int result);

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        internal delegate void GlGetFloatvDelegate(uint attr, out float result);

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        internal delegate uint GlGetErrorDelegate();

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        internal delegate bool GlIsEnabledDelegate(uint cap);

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        internal delegate void GlHintDelegate(uint target, uint mode);

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        internal delegate void GlLineWidthDelegate(float width);

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        internal delegate IntPtr GlGetStringiDelegate(uint attr, uint index);

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

        internal static GlGetFloatvDelegate GetFloatV =>
            Marshal.GetDelegateForFunctionPointer<GlGetFloatvDelegate>(
                SDL2.SDL_GL_GetProcAddress("glGetFloatv")
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

        internal static GlLineWidthDelegate LineWidth =>
            Marshal.GetDelegateForFunctionPointer<GlLineWidthDelegate>(
                SDL2.SDL_GL_GetProcAddress("glLineWidth")
            );

        internal static GlGetStringiDelegate GetStringI =>
            Marshal.GetDelegateForFunctionPointer<GlGetStringiDelegate>(
                SDL2.SDL_GL_GetProcAddress("glGetStringi")
            );
    }
}