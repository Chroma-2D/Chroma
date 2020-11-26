using System.Diagnostics;
using Chroma.Diagnostics.Logging.Base;

namespace Chroma.Diagnostics.Logging.Decorators
{
    public class ClassNameDecorator : Decorator
    {
        public override string Decorate(LogLevel logLevel, string input, string originalMessage, Sink sink)
        {
            var frame = 4;
            var name = "<>";

            var stackTrace = new StackTrace();
            while (name != null && (name.Contains("<") || name.Contains(">")))
            {
                frame++;

                name = stackTrace
                    .GetFrame(frame)
                    ?.GetMethod()
                    ?.DeclaringType
                    ?.Name;
            }

            return name;
        }
    }
}