using System;
using System.Runtime.InteropServices;
using Chroma.Natives.SDL;

namespace Chroma.Natives.GL
{
    public static class Gl
    {
        public const uint GL_MULTISAMPLE = 0x809D;
        
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        internal delegate void GlEnableDisableDelegate(uint extension);

        internal static GlEnableDisableDelegate GlEnable { get; }
        internal static GlEnableDisableDelegate GlDisable { get; }

        static Gl()
        {
            GlEnable = Marshal.GetDelegateForFunctionPointer<GlEnableDisableDelegate>(
                SDL2.SDL_GL_GetProcAddress("glEnable")
            );

            GlDisable = Marshal.GetDelegateForFunctionPointer<GlEnableDisableDelegate>(
                SDL2.SDL_GL_GetProcAddress("glDisable")
            );
        }

        public static void SwitchMultiSampleAA(bool enabled)
        {
            if (enabled) GlEnable(GL_MULTISAMPLE);
            else GlDisable(GL_MULTISAMPLE);
        }
    }
}