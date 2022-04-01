using System;
using System.Runtime.InteropServices;
using Chroma.NALO;
using Chroma.Natives.Bindings.SDL;

namespace Chroma.Natives.Boot
{
    internal class BootLog : EarlyLog
    {
        private SDL2.SDL_LogOutputFunction _defaultLogOutputFunction;
        
        public BootLog() 
            : base("crboot.log")
        {
        }

        internal void HookSdlLog()
        {
            SDL2.SDL_LogSetAllPriority(SDL2.SDL_LogPriority.SDL_LOG_PRIORITY_VERBOSE);
            SDL2.SDL_LogGetOutputFunction(out _defaultLogOutputFunction, out _);
            SDL2.SDL_LogSetOutputFunction(SdlLogOutputFunction, IntPtr.Zero);
        }

        protected override void Finish()
        {
            SDL2.SDL_LogSetOutputFunction(_defaultLogOutputFunction, IntPtr.Zero);
            _defaultLogOutputFunction = null;
        }
        
        private void SdlLogOutputFunction(
            IntPtr userdata,
            int category,
            SDL2.SDL_LogPriority priority,
            IntPtr message)
        {
            var messageString = Marshal.PtrToStringAuto(message);

            switch (priority)
            {
                case SDL2.SDL_LogPriority.SDL_LOG_PRIORITY_INFO:
                    Info(messageString);
                    break;

                case SDL2.SDL_LogPriority.SDL_LOG_PRIORITY_WARN:
                    Warning(messageString);
                    break;

                case SDL2.SDL_LogPriority.SDL_LOG_PRIORITY_ERROR:
                case SDL2.SDL_LogPriority.SDL_LOG_PRIORITY_CRITICAL:
                    Error(messageString);
                    break;

                case SDL2.SDL_LogPriority.SDL_LOG_PRIORITY_DEBUG:
                case SDL2.SDL_LogPriority.SDL_LOG_PRIORITY_VERBOSE:
                    Debug(messageString);
                    break;
            }
        }
    }
}