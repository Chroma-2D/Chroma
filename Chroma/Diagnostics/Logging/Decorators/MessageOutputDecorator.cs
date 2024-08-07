﻿namespace Chroma.Diagnostics.Logging.Decorators;

using Chroma.Diagnostics.Logging.Base;
using Chroma.Diagnostics.Logging.Sinks;

public class MessageOutputDecorator : Decorator
{
    public override string Decorate(LogLevel logLevel, string input, string originalMessage, Sink sink)
    {
        var output = originalMessage;

        if (sink is ConsoleSink)
            output = originalMessage.AnsiColorEncodeRGB(255, 255, 255);

        return output;
    }
}