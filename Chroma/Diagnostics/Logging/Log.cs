namespace Chroma.Diagnostics.Logging;

using System;
using System.Collections.Generic;
using System.Linq;
using Chroma.Diagnostics.Logging.Base;

public sealed class Log
{
    private bool HasBeenClosed { get; set; }

    public LogLevel LogLevel { get; set; }
    public string Template { get; private set; }

    internal List<Sink> Sinks { get; }
    internal Dictionary<string, Decorator> Decorators { get; }

    internal Log()
    {
        Sinks = new List<Sink>();
        Decorators = new Dictionary<string, Decorator>();

        LogLevel = LogLevel.Everything;
        Template = "{Message}";
    }

    public void Info(string message)
    {
        lock (this)
        {
            EnsureNotClosed();

            EnsureMinimalLogLevel(LogLevel.Info, (level) => { DecorateAndPushToAllActiveSinks(level, message); });
        }
    }

    public void Warning(string message)
    {
        lock (this)
        {
            EnsureNotClosed();

            EnsureMinimalLogLevel(LogLevel.Warning, (level) => { DecorateAndPushToAllActiveSinks(level, message); });
        }
    }

    public void Error(string message)
    {
        lock (this)
        {
            EnsureNotClosed();

            EnsureMinimalLogLevel(LogLevel.Error, (level) => { DecorateAndPushToAllActiveSinks(level, message); });
        }
    }

    public void Debug(string message)
    {
        lock (this)
        {
            EnsureNotClosed();

            EnsureMinimalLogLevel(LogLevel.Debug, (level) => { DecorateAndPushToAllActiveSinks(level, message); });
        }
    }

    public void Exception(Exception e)
    {
        lock (this)
        {
            EnsureNotClosed();

            EnsureMinimalLogLevel(LogLevel.Exception,
                (level) => { DecorateAndPushToAllActiveSinks(level, e.Message, e); });
        }
    }

    public void Info(object obj)
        => Info(obj.ToString() ?? "<null>");

    public void Warning(object obj)
        => Warning(obj.ToString() ?? "<null>");

    public void Error(object obj)
        => Error(obj.ToString() ?? "<null>");

    public void Debug(object obj)
        => Debug(obj.ToString() ?? "<null>");

    public Log WithOutputTemplate(string template)
    {
        EnsureNotClosed();

        Template = template;
        return this;
    }

    public Log SinkTo<T>() where T : Sink, new()
    {
        EnsureNotClosed();

        if (SinkExists<T>())
            throw new DuplicateSinkException(typeof(T));

        Sinks.Add(new T());
        return this;
    }

    public Log SinkTo(Sink sink)
    {
        EnsureNotClosed();

        var sinkType = sink.GetType();

        if (SinkExists(sinkType))
            throw new DuplicateSinkException(sinkType);

        Sinks.Add(sink);

        return this;
    }

    public Log DecorateWith<T>(string template) where T : Decorator, new()
    {
        EnsureNotClosed();

        Decorators.Add($"{{{template}}}", new T());
        return this;
    }

    public Log DecorateWith(Decorator decorator, string template)
    {
        EnsureNotClosed();

        Decorators.Add($"{{{template}}}", decorator);
        return this;
    }

    public void Close()
    {
        EnsureNotClosed();

        foreach (var sink in Sinks)
        {
            if (sink is IDisposable disposable)
            {
                disposable.Dispose();
            }

            sink.Active = false;
        }

        Sinks.Clear();
        Decorators.Clear();

        HasBeenClosed = true;
    }

    private void DecorateAndPushToAllActiveSinks(LogLevel logLevel, string message, params object[] sinkArgs)
    {
        foreach (var sink in Sinks.Where(x => x.Active))
        {
            var actualOutput = Template;

            foreach (var kvp in Decorators)
            {
                actualOutput = actualOutput.Replace(
                    kvp.Key,
                    kvp.Value.Decorate(logLevel, actualOutput, message, sink)
                );
            }

            sink.Write(logLevel, actualOutput, sinkArgs);
        }
    }

    private void EnsureMinimalLogLevel(LogLevel logLevel, Action<LogLevel> logAction)
    {
        if (((int)logLevel & (int)LogLevel) != 0)
            logAction(logLevel);
    }

    private void EnsureNotClosed()
    {
        if (HasBeenClosed)
            throw new InvalidOperationException("This log has already been closed.");
    }

    private bool SinkExists(Type type)
        => Sinks.Exists(x => x.GetType() == type);

    private bool SinkExists<T>()
        => SinkExists(typeof(T));
}