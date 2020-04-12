using System.IO;

namespace Chroma.Diagnostics.Logging.Sinks
{
    public class FileSink : StreamSink
    {
        public FileSink(string filePath)
            : base(new FileStream(
                filePath, FileMode.OpenOrCreate, FileAccess.Write, FileShare.Read
              ))
        { }

        public override void Write(LogLevel logLevel, string message, params object[] args)
        {
            base.Write(logLevel, message, args);
        }
    }
}
