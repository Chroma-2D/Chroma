namespace Chroma.Diagnostics.Logging.Sinks;

using System;
using System.IO;
using Chroma.Diagnostics.Logging.Base;

public class StreamSink(Stream stream)
    : Sink, IDisposable
{
    private StreamWriter StreamWriter { get; } = new(stream) { AutoFlush = true };

    public override void Write(LogLevel logLevel, string message, params object[] args)
    {
        StreamWriter.WriteLine(message);

        if (args is [Exception e])
        {
            StreamWriter.WriteLine(
                Formatting.ExceptionForLogging(e, true)
            );
        }
    }

    public void Dispose()
    {
        StreamWriter?.Dispose();
    }
}