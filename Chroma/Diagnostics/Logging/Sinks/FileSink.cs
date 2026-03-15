namespace Chroma.Diagnostics.Logging.Sinks;

using System.IO;

public class FileSink(string filePath) : StreamSink(new FileStream(
    filePath, FileMode.OpenOrCreate, FileAccess.Write, FileShare.Read
));