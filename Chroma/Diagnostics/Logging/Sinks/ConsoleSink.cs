using Chroma.Diagnostics.Logging.Base;
using System;
using System.Reflection;

namespace Chroma.Diagnostics.Logging.Sinks
{
    public class ConsoleSink : Sink
    {
        public override void Write(LogLevel logLevel, string message, params object[] args)
        {
            Console.WriteLine(message);

            if (args.Length == 1)
            {
                if (args[0] is ReflectionTypeLoadException rtle)
                {
                    Console.WriteLine(
                        Formatting.ReflectionTypeLoadExceptionForLogging(rtle)
                    );
                }
                else if (args[0] is Exception e)
                {
                    Console.WriteLine(
                        Formatting.ExceptionForLogging(e, true)
                    );
                }
            }
        }
    }
}
