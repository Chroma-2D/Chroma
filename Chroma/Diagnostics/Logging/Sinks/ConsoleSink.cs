using System;
using Chroma.Diagnostics.Logging.Base;

namespace Chroma.Diagnostics.Logging.Sinks
{
    public class ConsoleSink : Sink
    {
        public override void Write(LogLevel logLevel, string message, params object[] args)
        {
            Console.WriteLine(message);

            if (args.Length == 1)
            {
                if (args[0] is Exception e)
                {
                    Console.WriteLine(
                        Formatting.ExceptionForLogging(e, true)
                    );
                }
            }
        }
    }
}