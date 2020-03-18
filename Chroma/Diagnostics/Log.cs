using System;
using System.Diagnostics;

namespace Chroma.Diagnostics
{
    public class Log
    {
        public static Verbosity Verbosity { get; set; } = Verbosity.Standard;

        public static void Info(string message)
        {
            if (!Verbosity.HasFlag(Verbosity.Info))
                return;

            PushMessage("INF", message, CallerTypeName());
        }

        public static void Warning(string message)
        {
            if (!Verbosity.HasFlag(Verbosity.Warning))
                return;

            PushMessage("WRN", message, CallerTypeName());
        }

        public static void Error(string message)
        {
            if (!Verbosity.HasFlag(Verbosity.Error))
                return;

            PushMessage("ERR", message, CallerTypeName());
        }

        public static void Debug(string message)
        {
            if (!Verbosity.HasFlag(Verbosity.Debug))
                return;

            PushMessage("DBG", message, CallerTypeName());
        }

        private static string CallerTypeName()
        {
            var trace = new StackTrace();
            var frame = trace.GetFrame(2);

            return frame.GetMethod().DeclaringType.FullName;
        }

        private static void PushMessage(string descriptor, string message, string context)
        {
            Console.WriteLine($"[{DateTime.Now:HH:mm:ss} {descriptor}] [{context}] {message}");
        }
    }
}
