using System;
using System.Globalization;
using System.IO;

namespace Chroma.NALO
{
    public class EarlyLog : IDisposable
    {
        private StreamWriter _streamWriter;

        public bool Enabled { get; set; } = true;

        public EarlyLog(string outFileName)
        {
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

        public void Info(string message)
        {
            WriteToLog(message, "INF");
        }

        public void Warning(string message)
        {
            WriteToLog(message, "WRN");
        }

        public void Error(string message)
        {
            WriteToLog(message, "ERR");
        }

        public void Debug(string message)
        {
            WriteToLog(message, "DBG");
        }
        
        public void Dispose()
        {
            _streamWriter.Dispose();
            Finish();
        }

        protected virtual void Finish()
        {
        }

        private void WriteToLog(string message, string level)
        {
            if (!Enabled)
                return;

            var msg = $"[{DateTime.Now.ToString("HH:mm:ss", CultureInfo.InvariantCulture)} {level}] {message}";

            if (_streamWriter != null)
                _streamWriter.WriteLine(msg);

            Console.WriteLine(msg);
        }
    }
}