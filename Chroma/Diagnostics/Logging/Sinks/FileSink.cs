namespace Chroma.Diagnostics.Logging.Sinks;

using System.IO;

public class FileSink : StreamSink
{
    public FileSink(string filePath)
        : base(new FileStream(
            filePath, FileMode.OpenOrCreate, FileAccess.Write, FileShare.Read
        ))
    {
    }
}