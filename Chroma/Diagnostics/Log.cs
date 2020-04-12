using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace Chroma.Diagnostics
{
    public static class Log
    {
        private static readonly HashSet<LogSinkDelegate> LogSinks = new HashSet<LogSinkDelegate>();

        public delegate void LogSinkDelegate(string message, Verbosity verbosity);
        
        public static void Info(string message)
            => PushMessage("INF", message, CallerTypeName(), Verbosity.Info);

        public static void Warning(string message)
            => PushMessage("WRN", message, CallerTypeName(), Verbosity.Warning);

        public static void Error(string message)
            => PushMessage("ERR", message, CallerTypeName(), Verbosity.Error);

        public static void Debug(string message)
            => PushMessage("DBG", message, CallerTypeName(), Verbosity.Debug);

        public static void AddLogSink(LogSinkDelegate sink)
            => LogSinks.Add(sink);

        public static void RemoveLogSink(LogSinkDelegate sink)
            => LogSinks.Remove(sink);

        public static void RemoveAllSinks()
            => LogSinks.RemoveWhere(x => true);

        private static string CallerTypeName()
        {
            var trace = new StackTrace();
            var frame = trace.GetFrame(2);

            return frame?.GetMethod()?.DeclaringType?.Name;
        }

        private static void PushMessage(string descriptor, string message, string context, Verbosity verbosity)
        {
            foreach (var sink in LogSinks)
                sink?.Invoke($"[{DateTime.Now:HH:mm:ss} {descriptor}] [{context}] {message}", verbosity);
        }
    }
}