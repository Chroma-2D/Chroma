using System;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Runtime.InteropServices;
using Chroma.Natives.SDL;

namespace Chroma.Natives.Boot
{
    internal static class BootLog
    {
        private static bool _enabled;

        private static SDL2.SDL_LogOutputFunction _defaultLogOutputFunction;
        private static StreamWriter _streamWriter;

        internal static void Begin()
        {
            if (_enabled)
                return;

            try
            {
                var logDir = Path.Combine(AppContext.BaseDirectory, "Logs");

                if (!Directory.Exists(logDir))
                    Directory.CreateDirectory(logDir);

                var logPath = Path.Combine(logDir, "crboot.log");
                _streamWriter = new StreamWriter(logPath) { AutoFlush = true };
            }
            catch (Exception e)
            {
                Console.WriteLine(
                    $"Failed to open a file for early boot log: {e.Message} " +
                    "Log will only be available in the console."
                );
            }

            _enabled = true;
        }

        internal static void HookSdlLog()
        {
            SDL2.SDL_LogSetAllPriority(SDL2.SDL_LogPriority.SDL_LOG_PRIORITY_VERBOSE);
            SDL2.SDL_LogGetOutputFunction(out _defaultLogOutputFunction, out _);
            SDL2.SDL_LogSetOutputFunction(SdlLogOutputFunction, IntPtr.Zero);
        }

        internal static void Info(string message)
        {
            WriteToLog(message, "INF");
        }

        internal static void Warning(string message)
        {
            WriteToLog(message, "WRN");
        }

        internal static void Error(string message)
        {
            WriteToLog(message, "ERR");
        }

        internal static void Debug(string message)
        {
            WriteToLog(message, "DBG");
        }

        internal static void End()
        {
            if (!_enabled)
                return;

            SDL2.SDL_LogSetOutputFunction(_defaultLogOutputFunction, IntPtr.Zero);
            _defaultLogOutputFunction = null;

            _streamWriter.Dispose();
            _streamWriter = null;
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

        private static void WriteToLog(string message, string level)
        {
            if (!_enabled)
                return;

            var msg = $"[{DateTime.Now.ToString("HH:mm:ss", CultureInfo.InvariantCulture)} {level}] {message}";

            if (_streamWriter != null)
                _streamWriter.WriteLine(msg);

            Console.WriteLine(msg);
        }
    }
}