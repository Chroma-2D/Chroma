using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;

namespace Chroma.NALO
{
    public static class EarlyLog
    {
        private static Queue<string> _messages = new();
        private static StreamWriter _streamWriter;

        public static bool Enabled { get; set; } = true;

        internal static void Begin(string outFileName)
        {
            if (_streamWriter != null)
                return;
            
            try
            {
                var logDir = Path.Combine(AppContext.BaseDirectory, "Logs");

                if (!Directory.Exists(logDir))
                    Directory.CreateDirectory(logDir);

                var logPath = Path.Combine(logDir, outFileName);
                _streamWriter = new StreamWriter(logPath) { AutoFlush = true };
            }
            catch (Exception e)
            {
                Console.WriteLine(
                    $"Failed to open a file for an early boot log '{outFileName}': {e.Message} " +
                    "Log will only be available in the console."
                );
            }
        }

        internal static void End()
        {
            _streamWriter.Dispose();
            _streamWriter = null;
        }
        
        public static void Info(string message)
        {
            _messages.Enqueue(FormatMessage(message, "INF"));
            WriteToLog();
        }

        public static void Warning(string message)
        {
            _messages.Enqueue(FormatMessage(message, "WRN"));
            WriteToLog();
        }

        public static void Error(string message)
        {
            _messages.Enqueue(FormatMessage(message, "ERR"));
            WriteToLog();
        }

        public static void Debug(string message)
        {
            _messages.Enqueue(FormatMessage(message, "DBG"));
            WriteToLog();
        }
        
        public static void Dispose()
        {
            _streamWriter.Dispose();
        }

        private static string FormatMessage(string message, string level)
        {
            return $"[{DateTime.Now.ToString("HH:mm:ss", CultureInfo.InvariantCulture)} {level}] {message}";
        }

        private static void WriteToLog()
        {
            while (_messages.Any())
            {
                var msg = _messages.Dequeue();

                if (Enabled)
                {
                    if (_streamWriter != null)
                        _streamWriter.WriteLine(msg);

                    Console.WriteLine(msg);
                }
            }
        }
    }
}