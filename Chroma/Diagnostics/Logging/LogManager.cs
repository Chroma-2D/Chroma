﻿namespace Chroma.Diagnostics.Logging;

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using Chroma.Diagnostics.Logging.Decorators;
using Chroma.Diagnostics.Logging.Sinks;

public static class LogManager
{
    private static string LogRoot { get; }
    private static List<LogInfo> Logs { get; }

    static LogManager()
    {
        LogRoot = Path.Combine(AppContext.BaseDirectory, "Logs");

        if (!Directory.Exists(LogRoot))
            Directory.CreateDirectory(LogRoot);

        Logs = new List<LogInfo>();
    }

    public static string GetCurrentAssemblyLogPath()
    {
        return GetAssemblyLogPath(
            Assembly.GetCallingAssembly()
        );
    }

    public static Log CreateBareLog()
        => new();

    public static Log GetForCurrentAssembly(bool initializeDefaults = true)
    {
        var asm = Assembly.GetCallingAssembly();

        return GetForAssembly(asm, initializeDefaults,
            (log) => { log.SinkTo(new FileSink(GetAssemblyLogPath(asm))); });
    }

    private static Log GetForAssembly(Assembly assembly, bool initializeDefaults, Action<Log>? postInit = null)
    {
        var logInfo = Logs.FirstOrDefault(x => x.OwningAssembly == assembly);

        if (logInfo == null)
        {
            var log = new Log();

            if (initializeDefaults)
            {
                log.WithOutputTemplate("[{DateTime} {LogLevel}] [{ClassName}] {Message}")
                    .DecorateWith<LogLevelDecorator>("LogLevel")
                    .DecorateWith<DateTimeDecorator>("DateTime")
                    .DecorateWith<ClassNameDecorator>("ClassName")
                    .DecorateWith<MessageOutputDecorator>("Message")
                    .SinkTo<ConsoleSink>();

                postInit?.Invoke(log);
            }

            logInfo = new LogInfo(assembly, log);
            Logs.Add(logInfo);
        }

        return logInfo.Log ?? throw new InvalidOperationException("");
    }

    private static string GetAssemblyLogPath(Assembly assembly)
        => Path.Combine(LogRoot, $"{assembly.GetName().Name}_{DateTime.Now:yyyy-MM-dd_hh_mm_ss}.log");
}