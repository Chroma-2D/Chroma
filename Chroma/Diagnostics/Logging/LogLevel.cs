namespace Chroma.Diagnostics.Logging;

using System;

[Flags]
public enum LogLevel
{
    Info = 1,
    Warning = 2,
    Error = 4,
    Debug = 8,
    Exception = 16,
    Everything = Info | Warning | Error | Debug | Exception
}