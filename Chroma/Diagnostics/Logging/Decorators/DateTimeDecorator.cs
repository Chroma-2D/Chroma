﻿namespace Chroma.Diagnostics.Logging.Decorators;

using System;
using System.Globalization;
using Chroma.Diagnostics.Logging.Base;

public class DateTimeDecorator : Decorator
{
    private DisplayMode Mode { get; }

    public DateTimeDecorator()
        : this(DisplayMode.TimeString24h)
    {
    }

    public DateTimeDecorator(DisplayMode mode)
    {
        Mode = mode;
    }

    public override string Decorate(LogLevel logLevel, string input, string originalMessage, Sink sink)
    {
        return Mode switch
        {
            DisplayMode.TimeString24h => DateTime.Now.ToString("HH:mm:ss", CultureInfo.InvariantCulture),
            DisplayMode.TimeString12h => DateTime.Now.ToString("hh:mm:ss tt", CultureInfo.InvariantCulture),
            DisplayMode.DefaultDateTimeString => DateTime.Now.ToString(CultureInfo.InvariantCulture),

            _ => "??:??:??"
        };
    }

    public enum DisplayMode
    {
        TimeString24h,
        TimeString12h,
        DefaultDateTimeString
    }
}