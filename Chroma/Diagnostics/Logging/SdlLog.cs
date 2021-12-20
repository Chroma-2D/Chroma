using System;
using System.Runtime.InteropServices;
using Chroma.Natives.SDL;

namespace Chroma.Diagnostics.Logging
{
    internal static class SdlLog
    {
        private static readonly Log _log = LogManager.GetForCurrentAssembly();
        private static SDL2.SDL_LogOutputFunction _originalLogOutputFunction;

        internal static void Enable()
        {
            if (_originalLogOutputFunction != null)
                return;
            
            SDL2.SDL_LogGetOutputFunction(out _originalLogOutputFunction, out _);
            SDL2.SDL_LogSetOutputFunction(LogOutputFunction, IntPtr.Zero);
        }

        private static void LogOutputFunction(
            IntPtr _, 
            int category, 
            SDL2.SDL_LogPriority priority,
            IntPtr message)
        {
            var messageString = Marshal.PtrToStringAuto(message);
            
            switch (priority)
            {
                case SDL2.SDL_LogPriority.SDL_LOG_PRIORITY_INFO:
                    _log.Info(messageString);
                    break;
                
                case SDL2.SDL_LogPriority.SDL_LOG_PRIORITY_WARN:
                    _log.Warning(messageString);
                    break;
                
                case SDL2.SDL_LogPriority.SDL_LOG_PRIORITY_ERROR:
                case SDL2.SDL_LogPriority.SDL_LOG_PRIORITY_CRITICAL:
                    _log.Error(messageString);
                    break;
                
                case SDL2.SDL_LogPriority.SDL_LOG_PRIORITY_DEBUG:
                case SDL2.SDL_LogPriority.SDL_LOG_PRIORITY_VERBOSE:
                    _log.Debug(messageString);
                    break;
                
            }
        }
    }
}