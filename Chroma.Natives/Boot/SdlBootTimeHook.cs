using System;
using System.Runtime.InteropServices;
using Chroma.NALO;
using Chroma.Natives.Bindings.SDL;

namespace Chroma.Natives.Boot
{
    internal static class SdlBootTimeHook
    {
        private static SDL2.SDL_LogOutputFunction _defaultLogOutputFunction;
        
        internal static void Hook()
        {
            SDL2.SDL_LogSetAllPriority(SDL2.SDL_LogPriority.SDL_LOG_PRIORITY_VERBOSE);
            SDL2.SDL_LogGetOutputFunction(out _defaultLogOutputFunction, out _);
            SDL2.SDL_LogSetOutputFunction(SdlLogOutputFunction, IntPtr.Zero);
        }

        internal static void UnHook()
        {
            SDL2.SDL_LogSetOutputFunction(_defaultLogOutputFunction, IntPtr.Zero);
            _defaultLogOutputFunction = null;
        }
        
        private static void SdlLogOutputFunction(
            IntPtr userdata,
            int category,
            SDL2.SDL_LogPriority priority,
            IntPtr message)
        {
            var messageString = Marshal.PtrToStringAuto(message);

            switch (priority)
            {
                case SDL2.SDL_LogPriority.SDL_LOG_PRIORITY_INFO:
                    EarlyLog.Info(messageString);
                    break;

                case SDL2.SDL_LogPriority.SDL_LOG_PRIORITY_WARN:
                    EarlyLog.Warning(messageString);
                    break;

                case SDL2.SDL_LogPriority.SDL_LOG_PRIORITY_ERROR:
                case SDL2.SDL_LogPriority.SDL_LOG_PRIORITY_CRITICAL:
                    EarlyLog.Error(messageString);
                    break;

                case SDL2.SDL_LogPriority.SDL_LOG_PRIORITY_DEBUG:
                case SDL2.SDL_LogPriority.SDL_LOG_PRIORITY_VERBOSE:
                    EarlyLog.Debug(messageString);
                    break;
            }
        }
    }
}